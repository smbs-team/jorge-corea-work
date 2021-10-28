// <copyright file="GenericSwaggerOperationAttribute.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Attributes
{
    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Annotation that allows to create unique operation ids for operations defined in "Generic" controllers.
    /// </summary>
    /// <seealso cref="Swashbuckle.AspNetCore.Annotations.SwaggerOperationAttribute" />
    public class GenericSwaggerOperationAttribute : SwaggerOperationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericSwaggerOperationAttribute" /> class.
        /// </summary>
        /// <param name="operationId">The operation identifier.</param>
        /// <param name="genericType">Type of the generic.</param>
        public GenericSwaggerOperationAttribute(string operationId)
        {
            string uniqueId = System.Guid.NewGuid().ToString().Replace("-", "_");
            this.OperationId = operationId + uniqueId;
        }
    }
}
