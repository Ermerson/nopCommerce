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
    /// Manufacturer template service
    /// </summary>
    public partial class ManufacturerTemplateService : IManufacturerTemplateService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<ManufacturerTemplate> _manufacturerTemplateRepository;
        private readonly IStaticCacheManager _staticCacheManager;

        #endregion

        #region Ctor

        public ManufacturerTemplateService(IEventPublisher eventPublisher,
            IRepository<ManufacturerTemplate> manufacturerTemplateRepository,
            IStaticCacheManager staticCacheManager)
        {
            _eventPublisher = eventPublisher;
            _manufacturerTemplateRepository = manufacturerTemplateRepository;
            _staticCacheManager = staticCacheManager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete manufacturer template
        /// </summary>
        /// <param name="manufacturerTemplate">Manufacturer template</param>
        public virtual void DeleteManufacturerTemplate(ManufacturerTemplate manufacturerTemplate)
        {
            if (manufacturerTemplate == null)
                throw new ArgumentNullException(nameof(manufacturerTemplate));

            _manufacturerTemplateRepository.Delete(manufacturerTemplate);

            //event notification
            _eventPublisher.EntityDeleted(manufacturerTemplate);
        }

        /// <summary>
        /// Gets all manufacturer templates
        /// </summary>
        /// <returns>Manufacturer templates</returns>
        public virtual IList<ManufacturerTemplate> GetAllManufacturerTemplates()
        {
            var query = from pt in _manufacturerTemplateRepository.Table
                        orderby pt.DisplayOrder, pt.Id
                        select pt;

            var templates = query.ToCachedList(_staticCacheManager.PrepareKeyForDefaultCache(NopCatalogDefaults.ManufacturerTemplatesAllCacheKey));

            return templates;
        }

        /// <summary>
        /// Gets a manufacturer template
        /// </summary>
        /// <param name="manufacturerTemplateId">Manufacturer template identifier</param>
        /// <returns>Manufacturer template</returns>
        public virtual ManufacturerTemplate GetManufacturerTemplateById(int manufacturerTemplateId)
        {
            if (manufacturerTemplateId == 0)
                return null;

            return _manufacturerTemplateRepository.GetById(manufacturerTemplateId);
        }

        /// <summary>
        /// Inserts manufacturer template
        /// </summary>
        /// <param name="manufacturerTemplate">Manufacturer template</param>
        public virtual void InsertManufacturerTemplate(ManufacturerTemplate manufacturerTemplate)
        {
            if (manufacturerTemplate == null)
                throw new ArgumentNullException(nameof(manufacturerTemplate));

            _manufacturerTemplateRepository.Insert(manufacturerTemplate);

            //event notification
            _eventPublisher.EntityInserted(manufacturerTemplate);
        }

        /// <summary>
        /// Updates the manufacturer template
        /// </summary>
        /// <param name="manufacturerTemplate">Manufacturer template</param>
        public virtual void UpdateManufacturerTemplate(ManufacturerTemplate manufacturerTemplate)
        {
            if (manufacturerTemplate == null)
                throw new ArgumentNullException(nameof(manufacturerTemplate));

            _manufacturerTemplateRepository.Update(manufacturerTemplate);

            //event notification
            _eventPublisher.EntityUpdated(manufacturerTemplate);
        }

        #endregion
    }
}