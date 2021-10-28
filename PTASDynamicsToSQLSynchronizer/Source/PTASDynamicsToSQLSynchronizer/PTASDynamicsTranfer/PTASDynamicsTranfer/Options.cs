﻿// <copyright file="Options.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>
// <auto-generated />

namespace PTASDynamicsTranfer
{
    using CommandLine;

    public class Options
    {
        private const int DefaultChunkSize = 1000;
        private const int DefaultUseBulk = 0;

        [Option('b', "bearer", Required = false, HelpText = "Bearer Token.")]
        public string BearerToken { get; set; }

        [Option('s', "chunksize", Default = DefaultChunkSize, Required = false, HelpText = "Chunk size (# of records to process)")]
        public int ChunkSize { get; set; } = DefaultChunkSize;

        [Option('c', "sqlconnection", Required = true, HelpText = "SQL Connection String.")]
        public string ConnectionString { get; set; }

        [Option('a', "authurl", Required = true, HelpText = "Auth URL. ex: https://login.windows.net/KC1.onmicrosoft.com")]
        public string AuthUri { get; set; }

        [Option('d', "dynamicsurl", Required = true, HelpText = "Dynamics URL.")]
        public string DynamicsURL { get; set; }

        [Option('e', "entityname", Required = true, HelpText = "Enter the entity name.")]
        public string EntityName { get; set; }

        [Option('i', "registrationid", Required = true, HelpText = "App Registration Id.")]
        public string ClientId { get; set; }

        [Option('x', "registrationsecret", Required = true, HelpText = "App Registration Secret.")]
        public string ClientSecret { get; set; }

        [Option('u', "bulk_copy", Default = DefaultUseBulk, Required = false, HelpText = "Use Bulk Copy?")]
        public int useBulkInsert { get; set; } = DefaultUseBulk;
    }
}