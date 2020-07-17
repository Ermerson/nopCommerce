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
    /// Review type service implementation
    /// </summary>
    public partial class ReviewTypeService : IReviewTypeService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<ProductReviewReviewTypeMapping> _productReviewReviewTypeMappingRepository;
        private readonly IRepository<ReviewType> _reviewTypeRepository;
        private readonly IStaticCacheManager _staticCacheManager;

        #endregion

        #region Ctor

        public ReviewTypeService(IEventPublisher eventPublisher,
            IRepository<ProductReviewReviewTypeMapping> productReviewReviewTypeMappingRepository,
            IRepository<ReviewType> reviewTypeRepository,
            IStaticCacheManager staticCacheManager)
        {
            _eventPublisher = eventPublisher;
            _productReviewReviewTypeMappingRepository = productReviewReviewTypeMappingRepository;
            _reviewTypeRepository = reviewTypeRepository;
            _staticCacheManager = staticCacheManager;
        }

        #endregion

        #region Methods

        #region Review type

        /// <summary>
        /// Gets all review types
        /// </summary>
        /// <returns>Review types</returns>
        public virtual IList<ReviewType> GetAllReviewTypes()
        {
            return _reviewTypeRepository.Table
                .OrderBy(reviewType => reviewType.DisplayOrder).ThenBy(reviewType => reviewType.Id)
                .ToCachedList(_staticCacheManager.PrepareKeyForDefaultCache(NopCatalogDefaults.ReviewTypeAllCacheKey));
        }

        /// <summary>
        /// Gets a review type 
        /// </summary>
        /// <param name="reviewTypeId">Review type identifier</param>
        /// <returns>Review type</returns>
        public virtual ReviewType GetReviewTypeById(int reviewTypeId)
        {
            if (reviewTypeId == 0)
                return null;
            
            return _reviewTypeRepository.GetById(reviewTypeId);
        }

        /// <summary>
        /// Inserts a review type
        /// </summary>
        /// <param name="reviewType">Review type</param>
        public virtual void InsertReviewType(ReviewType reviewType)
        {
            if (reviewType == null)
                throw new ArgumentNullException(nameof(reviewType));

            _reviewTypeRepository.Insert(reviewType);

            //event notification
            _eventPublisher.EntityInserted(reviewType);
        }

        /// <summary>
        /// Updates a review type
        /// </summary>
        /// <param name="reviewType">Review type</param>
        public virtual void UpdateReviewType(ReviewType reviewType)
        {
            if (reviewType == null)
                throw new ArgumentNullException(nameof(reviewType));

            _reviewTypeRepository.Update(reviewType);

            //event notification
            _eventPublisher.EntityUpdated(reviewType);
        }

        /// <summary>
        /// Delete review type
        /// </summary>
        /// <param name="reviewType">Review type</param>
        public virtual void DeleteReiewType(ReviewType reviewType)
        {
            if (reviewType == null)
                throw new ArgumentNullException(nameof(reviewType));

            _reviewTypeRepository.Delete(reviewType);

            //event notification
            _eventPublisher.EntityDeleted(reviewType);
        }

        #endregion

        #region Product review type mapping

        /// <summary>
        /// Gets product review and review type mappings by product review identifier
        /// </summary>
        /// <param name="productReviewId">The product review identifier</param>
        /// <returns>Product review and review type mapping collection</returns>
        public IList<ProductReviewReviewTypeMapping> GetProductReviewReviewTypeMappingsByProductReviewId(
            int productReviewId)
        {
            var key = _staticCacheManager.PrepareKeyForDefaultCache(NopCatalogDefaults.ProductReviewReviewTypeMappingAllCacheKey, productReviewId);

            var query = from pam in _productReviewReviewTypeMappingRepository.Table
                orderby pam.Id
                where pam.ProductReviewId == productReviewId
                select pam;
            var productReviewReviewTypeMappings = query.ToCachedList(key);

            return productReviewReviewTypeMappings;
        }

        /// <summary>
        /// Inserts a product review and review type mapping
        /// </summary>
        /// <param name="productReviewReviewType">Product review and review type mapping</param>
        public virtual void InsertProductReviewReviewTypeMappings(ProductReviewReviewTypeMapping productReviewReviewType)
        {
            if (productReviewReviewType == null)
                throw new ArgumentNullException(nameof(productReviewReviewType));

            _productReviewReviewTypeMappingRepository.Insert(productReviewReviewType);

            //event notification
            _eventPublisher.EntityInserted(productReviewReviewType);
        }

        #endregion

        #endregion
    }
}