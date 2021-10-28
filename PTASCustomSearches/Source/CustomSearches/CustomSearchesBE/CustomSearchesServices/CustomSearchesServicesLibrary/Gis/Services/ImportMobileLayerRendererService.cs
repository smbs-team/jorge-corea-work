namespace CustomSearchesServicesLibrary.Gis.Services
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.Gis;
    using CustomSearchesEFLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.ServiceFramework;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Microsoft.WindowsAzure.Storage.File;

    /// <summary>
    /// Service that imports a new mobile layer renderer.
    /// </summary>
    /// <seealso cref="CustomSearchesServicesLibrary.ServiceFramework.BaseService" />
    public class ImportMobileLayerRendererService : BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportMobileLayerRendererService"/> class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public ImportMobileLayerRendererService(IServiceContext serviceContext)
            : base(serviceContext)
        {
        }

        /// <summary>
        /// Imports a mobile layer renderer.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        /// <param name="mobileLayerRendererData">Mobile renderer layer data.</param>
        /// <returns>
        /// The task that will return back the new mobile layer renderer guid or the updated mobile layer renderer guid.
        /// </returns>
        public async Task<Guid> ImportMobileLayerRenderer(GisDbContext dbContext, MobileLayerRendererData mobileLayerRendererData)
        {
            Guid userId = this.ServiceContext.AuthProvider.UserInfoData.Id; // I think will be needed at some point
            mobileLayerRendererData.Name = mobileLayerRendererData.Name.Trim();
            InputValidationHelper.AssertZero(mobileLayerRendererData.LayerSourceId, nameof(LayerSource), nameof(mobileLayerRendererData.LayerSourceId));
            InputValidationHelper.AssertNotEmpty(mobileLayerRendererData.Name, nameof(mobileLayerRendererData.Name));
            InputValidationHelper.AssertNotEmpty(mobileLayerRendererData.Query, nameof(mobileLayerRendererData.Query));

            string errorMessage = string.Empty;
            if (mobileLayerRendererData.RendererRules == null)
            {
                errorMessage = "RendererRules are required!";
                throw new CustomSearchesRequestBodyException(errorMessage, null);
            }

            this.ServiceContext.AuthProvider.AuthorizeAdminRole("ImportLayerSource");

            MobileLayerRenderer existingRenderer =
                await (from ls in dbContext.MobileLayerRenderer
                       where ls.Name.ToLower() == mobileLayerRendererData.Name.ToLower()
                       select ls).FirstOrDefaultAsync();

            MobileLayerRenderer savedMobileLayerRenderer = null;
            if (existingRenderer == null)
            {
                savedMobileLayerRenderer = mobileLayerRendererData.ToEfModel();
                dbContext.MobileLayerRenderer.Add(savedMobileLayerRenderer);
            }
            else
            {
                savedMobileLayerRenderer = existingRenderer;
                mobileLayerRendererData.UpdateEFModel(savedMobileLayerRenderer);
            }

            await dbContext.ValidateAndSaveChangesAsync();

            return savedMobileLayerRenderer.MobileLayerRendererId;
        }
    }
}