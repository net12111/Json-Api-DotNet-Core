using System.Collections.Immutable;
using JsonApiDotNetCore.Configuration;
using JsonApiDotNetCore.Queries;
using JsonApiDotNetCore.Queries.Internal;
using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;

namespace JsonApiDotNetCore.MongoDb.Queries.Internal;

/// <inheritdoc />
public sealed class HideRelationshipsSparseFieldSetCache : ISparseFieldSetCache
{
    private readonly SparseFieldSetCache _innerCache;

    public HideRelationshipsSparseFieldSetCache(IEnumerable<IQueryConstraintProvider> constraintProviders,
        IResourceDefinitionAccessor resourceDefinitionAccessor)
    {
        ArgumentGuard.NotNull(constraintProviders, nameof(constraintProviders));
        ArgumentGuard.NotNull(resourceDefinitionAccessor, nameof(resourceDefinitionAccessor));

        _innerCache = new SparseFieldSetCache(constraintProviders, resourceDefinitionAccessor);
    }

    /// <inheritdoc />
    public IImmutableSet<ResourceFieldAttribute> GetSparseFieldSetForQuery(ResourceType resourceType)
    {
        return _innerCache.GetSparseFieldSetForQuery(resourceType);
    }

    /// <inheritdoc />
    public IImmutableSet<AttrAttribute> GetIdAttributeSetForRelationshipQuery(ResourceType resourceType)
    {
        return _innerCache.GetIdAttributeSetForRelationshipQuery(resourceType);
    }

    /// <inheritdoc />
    public IImmutableSet<ResourceFieldAttribute> GetSparseFieldSetForSerializer(ResourceType resourceType)
    {
        IImmutableSet<ResourceFieldAttribute> fieldSet = _innerCache.GetSparseFieldSetForSerializer(resourceType);

        ResourceFieldAttribute[] relationships = fieldSet.Where(field => field is RelationshipAttribute).ToArray();
        return fieldSet.Except(relationships);
    }

    /// <inheritdoc />
    public void Reset()
    {
        _innerCache.Reset();
    }
}