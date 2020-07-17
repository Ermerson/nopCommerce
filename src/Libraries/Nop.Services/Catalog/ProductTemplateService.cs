using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Core.Events;
using Nop.Data;
using Nop.Services.Caching.Extensions;

namespace Nop.Services.Catalog
{
    /// <summary>
    /// Product template service
    /// </summary>
    public partial class ProductTemplateService : IProductTemplateService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<ProductTemplate> _productTemplateRepository;
        private readonly IStaticCacheManager _staticCacheManager;

        #endregion

        #region Ctor

        public ProductTemplateService(IEventPublisher eventPublisher,
            IRepository<ProductTemplate> productTemplateRepository,
            IStaticCacheManager staticCacheManager)
        {
            _eventPublisher = eventPublisher;
            _productTemplateRepository = productTemplateRepository;
            _staticCacheManager = staticCacheManager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete product template
        /// </summary>
        /// <param name="productTemplate">Product template</param>
        public virtual void DeleteProductTemplate(ProductTemplate productTemplate)
        {
            if (productTemplate == null)
                throw new ArgumentNullException(nameof(productTemplate));

            _productTemplateRepository.Delete(productTemplate);

            //event notification
            _eventPublisher.EntityDeleted(productTemplate);
        }

        /// <summary>
        /// Gets all product templates
        /// </summary>
        /// <returns>Product templates</returns>
        public virtual IList<ProductTemplate> GetAllProductTemplates()
        {
            var query = from pt in _productTemplateRepository.Table
                        orderby pt.DisplayOrder, pt.Id
                        select pt;

            var templates = query.ToCachedList(_staticCacheManager.PrepareKeyForDefaultCache(NopCatalogDefaults.ProductTemplatesAllCacheKey));

            return templates;
        }

        /// <summary>
        /// Gets a product template
        /// </summary>
        /// <param name="productTemplateId">Product template identifier</param>
        /// <returns>Product template</returns>
        public virtual ProductTemplate GetProductTemplateById(int productTemplateId)
        {
            if (productTemplateId == 0)
                return null;

            return _productTemplateRepository.GetById(productTemplateId);
        }

        /// <summary>
        /// Inserts product template
        /// </summary>
        /// <param name="productTemplate">Product template</param>
        public virtual void InsertProductTemplate(ProductTemplate productTemplate)
        {
            if (productTemplate == null)
                throw new ArgumentNullException(nameof(productTemplate));

            _productTemplateRepository.Insert(productTemplate);

            //event notification
            _eventPublisher.EntityInserted(productTemplate);
        }

        /// <summary>
        /// Updates the product template
        /// </summary>
        /// <param name="productTemplate">Product template</param>
        public virtual void UpdateProductTemplate(ProductTemplate productTemplate)
        {
            if (productTemplate == null)
                throw new ArgumentNullException(nameof(productTemplate));

            _productTemplateRepository.Update(productTemplate);

            //event notification
            _eventPublisher.EntityUpdated(productTemplate);
        }

        #endregion
    }
}