// Copyright (C) 2021-2022 Ubiquitous AS. All rights reserved
// Licensed under the Apache License, Version 2.0.

using Eventuous.Subscriptions.Context;

namespace Eventuous.Projections.MongoDB;

public delegate string GetDocumentId(StreamName evt);

public delegate UpdateDefinition<T> BuildUpdate<T>(UpdateDefinitionBuilder<T> update);

public delegate UpdateDefinition<T> BuildUpdate<in TEvent, T>(TEvent evt, UpdateDefinitionBuilder<T> update);

public delegate ValueTask<UpdateDefinition<T>> BuildUpdateAsync<in TEvent, T>(TEvent evt, UpdateDefinitionBuilder<T> update);

public delegate FilterDefinition<T> BuildFilter<T>(FilterDefinitionBuilder<T> filter);

public delegate FilterDefinition<T> BuildFilter<in TEvent, T>(IMessageConsumeContext<TEvent> ctx, FilterDefinitionBuilder<T> filter) where TEvent : class;

public delegate IndexKeysDefinition<T> BuildIndex<T>(IndexKeysDefinitionBuilder<T> builder);
    
public delegate UpdateDefinition<T> BuildBulkUpdate<T>(T document, UpdateDefinitionBuilder<T> update);

public delegate FilterDefinition<T> BuildBulkFilter<T>(T document, FilterDefinitionBuilder<T> filter);