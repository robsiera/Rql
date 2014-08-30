using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using System.Collections.ObjectModel;
using Rql;

namespace Rql.MongoDB
{
    public class SortSpecToMongoException : Exception
    {
        public SortSpecToMongoException(string message) : base(message)
        {
        }
    }

    public class SortSpecToMongoSortByCompiler
    {
        public SortSpecToMongoSortByCompiler()
        {
        }

        public IMongoSortBy Compile(string sortSpec)
        {
            if (String.IsNullOrEmpty(sortSpec))
                return SortBy.Null;

            return Compile(new SortSpecParser().Parse(sortSpec));
        }

        public IMongoSortBy Compile(SortSpec sortSpec)
        {
            var builder = new SortByBuilder();

            foreach (var field in sortSpec.Fields)
            {
                var fieldName = field.Name;

                if (String.CompareOrdinal(field.Name, "$natural") != 0)
                {
                    fieldName = MongoNameFixer.Field(field.Name);
                }

                if (field.Order == SortSpecSortOrder.Ascending)
                    builder.Ascending(MongoUtils.ToCamelCase(fieldName));
                else if (field.Order == SortSpecSortOrder.Descending)
                    builder.Descending(MongoUtils.ToCamelCase(fieldName));
            }

            return (IMongoSortBy)builder;
        }
    }
}

