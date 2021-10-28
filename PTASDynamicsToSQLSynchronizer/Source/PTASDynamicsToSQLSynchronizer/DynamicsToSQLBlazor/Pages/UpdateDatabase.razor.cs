// <copyright file="UpdateDatabase.razor.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace DynamicsToSQLBlazor.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using BlazorStrap;

    using D2SSyncHelpers.Models;
    using D2SSyncHelpers.Services;

    using Microsoft.AspNetCore.Components;
    using Microsoft.JSInterop;

    /// <summary>
    /// Update database component.
    /// </summary>
    public partial class UpdateDatabase
    {
        private BSModal AddTablesModal { get; set; }

        private IDictionary<string, bool> CheckedTables { get; set; } = new Dictionary<string, bool>() { };

        private bool CheckingTables { get; set; } = false;

        private BSModal CheckTablesModal { get; set; }

        private BSModal ExecuteQueryModal { get; set; }

        private bool LoadingXML { get; set; }

        private string Message { get; set; }

        private string QueryToRun { get; set; } = string.Empty;

        private BSModal RebuildFKsModal { get; set; }

        private bool Rebuilding { get; set; }

        private string SearchFilter { get; set; } = string.Empty;

        private DBTable[] SelectedTables { get; set; } = null;

        private string TableBeingChecked { get; set; } = string.Empty;

        private DBTable[] Tables { get; set; } = null;

        private List<string> TablesToAdd { get; set; } = new List<string>();

        private string TablesToModify { get; set; }

        private DBTable[] XMLTables { get; set; } = null;

        /// <inheritdoc/>
        protected override async Task OnInitializedAsync()
        {
            await this.GetInfo();
        }

        private static string CreateForeignKeyScript(EntityReference entityRef, DBTable dbTable)
        {
            var fkName = $"FK_{entityRef.ReferencedEntity}_{entityRef.ReferencingEntity}_{entityRef.ReferencingAttribute}";
            return @$"
                IF NOT EXISTS(select * from sysobjects where name = '{fkName}') BEGIN
                    ALTER TABLE dynamics.{dbTable.Name}
                    ADD CONSTRAINT {fkName}
                    FOREIGN KEY (_{entityRef.ReferencingAttribute}_value) REFERENCES dynamics.{entityRef.ReferencedEntity}({entityRef.ReferencedAttribute})
                    ON DELETE NO ACTION
                    ON UPDATE NO ACTION
                    NOT FOR REPLICATION;
                    ALTER TABLE dynamics.{dbTable.Name}
                    NOCHECK CONSTRAINT {fkName}
                END";
        }

        private async Task AddEntity()
        {
            this.TablesToAdd.Clear();
            await this.GetInfo();
            await this.LoadXMLTables();
            this.SearchFilter = string.Empty;
            this.AddTablesModal.Show();
        }

        private void CheckboxClicked(DBTable table, object checkedValue)
        {
            if ((bool)checkedValue)
            {
                this.CheckedTables[table.Name] = true;
            }
            else
            {
                this.CheckedTables.Remove(table.Name);
            }
        }

        private void CheckTables()
        {
            this.SelectedTables = this.Tables.Where(t => this.IsChecked(t.Name)).ToArray();
            this.CheckTablesModal.Show();
        }

        private async Task CreateTable()
        {
            try
            {
                var tables = this.TablesToAdd.Select(s => this.XMLTables.Where(t => t.Name == s).SingleOrDefault());
                foreach (var item in tables)
                {
                    await this.TableService.CreateTable(item);
                }

                this.Message = $"Finished creating {tables.Count()} table(s)";
                await this.GetInfo();
                this.TablesToAdd.Clear();
            }
            catch (Exception ex)
            {
                this.Message = ex.Message;
            }

            this.AddTablesModal.Hide();
        }

        private async Task DoCheckTables()
        {
            this.CheckingTables = true;
            var dbTables = (await this.TableService.GetTables()).Where(t => this.IsChecked(t.Name));
            var allTables = await this.XMLService.GetTables();
            var results = new List<string>();
            var tablesModified = string.Join(", ", dbTables.Select(s => s.Name));
            foreach (var table in dbTables)
            {
                this.TableBeingChecked = table.Name;
                this.StateHasChanged();

                var foundTable = allTables.FirstOrDefault(x => x.Name == table.Name);
                var newFields = FieldsComparer.NewFields(foundTable, table);
                if (newFields.Any())
                {
                    results.Add(SQLGenerator.AddFieldsScript(table.Name, newFields));
                }

                if (!this.CheckingTables)
                {
                    break;
                }
            }

            this.CheckTablesModal.Hide();

            if (this.CheckingTables && results.Any())
            {
                var s = string.Join(";\n\n", results) + ";\n\n";
                this.QueryToRun = s;
                this.TablesToModify = tablesModified;
                this.ExecuteQueryModal.Show();
            }
            else
            {
                this.Message = "Nothing to do.";
            }

            this.CheckingTables = false;
        }

        private async Task DoExecuteQuery()
        {
            try
            {
                await this.Database.SaveData<object>(this.QueryToRun, null);
                this.Message = this.QueryToRun + "\n" + "Saved Tables...";
            }
            catch (Exception ex)
            {
                this.Message = ex.Message;
            }

            this.ExecuteQueryModal.Hide();
        }

        private void DoSearch(EventArgs e)
        {
            this.StateHasChanged();
        }

        [JSInvokable]
        private async Task GetInfo()
        {
            this.Tables = (await this.TableService.GetTables()).ToArray();
        }

        private IEnumerable<DBTable> GetTablesToAdd()
        {
            if (this.XMLTables != null && this.TablesToAdd != null)
            {
                return this.XMLTables.Join(this.TablesToAdd, table => table.Name, tableName => tableName, (table, b) => table);
            }

            return new DBTable[] { };
        }

        private DBTable[] GetXMLTables()
            => string.IsNullOrEmpty(this.SearchFilter)
                ? this.XMLTables
                : this.XMLTables.Where(x => x.Name.Contains(this.SearchFilter, StringComparison.InvariantCultureIgnoreCase)).ToArray();

        private bool IsChecked(string name)
        {
            return this.CheckedTables.ContainsKey(name);
        }

        private async Task LoadXMLTables()
        {
            this.LoadingXML = true;
            this.StateHasChanged();
            this.XMLTables = (await this.XMLService.GetTables())
            .Where(f => !this.Tables.Any(t => t.Name.Equals(f.Name))).ToArray();
            this.LoadingXML = false;
        }

        private bool NoneSelected()
        {
            return !this.CheckedTables.Any();
        }

        private async Task RebuildFKs()
        {
            var msg = new StringBuilder();
            try
            {
                var tablesToBuild = await Task.FromResult(this.Tables.ToArray());
                this.RebuildFKsModal.Show();
                this.Rebuilding = true;
                var count = tablesToBuild.Count();
                var current = 0;
                foreach (var item in tablesToBuild)
                {
                    try
                    {
                        this.TableBeingChecked = $"{item.Name} {++current}/{count}";
                        this.StateHasChanged();

                        if (!this.Rebuilding)
                        {
                            break;
                        }

                        var relationships = await this.RelationsLoader.GetRelatedEntities(item.Name);

                        var tasks = relationships
                            .Where(relation => this.Tables.Any(ttt => ttt.Name == relation.ReferencedEntity))
                            .Select(relation => CreateForeignKeyScript(relation, item))
                            .Select(script => this.Database.SaveData<object>(script, null));

                        if (!this.Rebuilding)
                        {
                            break;
                        }

                        await Task.WhenAll(tasks);
                    }
                    catch (Exception ex)
                    {
                        msg.AppendLine(ex.Message);
                    }
                }

                if (msg.Length > 0)
                {
                    this.Message = msg.ToString();
                }
            }
            catch (Exception ex)
            {
                this.Message = ex.Message;
            }
            finally
            {
                this.RebuildFKsModal.Hide();
            }
        }

        private void SelectAll(ChangeEventArgs e)
        {
            if (true.Equals(e.Value))
            {
                this.CheckedTables = this.Tables.ToDictionary(t => t.Name, t => true);
            }
            else
            {
                this.CheckedTables.Clear();
            }
        }

        private void SelectAllDynamicsTables(ChangeEventArgs e)
        {
            if (true.Equals(e.Value))
            {
                this.TablesToAdd = this.GetXMLTables().Select(e => e.Name).ToList();
            }
            else
            {
                this.TablesToAdd.Clear();
            }
        }

        private async Task StopCheck()
        {
            this.CheckingTables = false;
            await Task.FromResult(0);
        }

        private async Task StopRebuilding()
        {
            this.Rebuilding = false;
            await Task.FromResult(0);
        }

        private void ToggleTable(ChangeEventArgs e, string name)
        {
            var c = true.Equals(e.Value);
            if (!c)
            {
                this.TablesToAdd.Remove(name);
            }
            else
            {
                this.TablesToAdd.Add(name);
            }
        }
    }
}