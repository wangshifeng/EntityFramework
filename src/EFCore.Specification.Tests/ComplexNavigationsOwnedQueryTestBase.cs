﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using Microsoft.EntityFrameworkCore.Specification.Tests.TestModels.ComplexNavigationsModel;
using Microsoft.EntityFrameworkCore.Specification.Tests.TestUtilities.Xunit;

namespace Microsoft.EntityFrameworkCore.Specification.Tests
{
    public abstract class ComplexNavigationsOwnedQueryTestBase<TTestStore, TFixture> : ComplexNavigationsQueryTestBase<TTestStore, TFixture>
        where TTestStore : TestStore
        where TFixture : ComplexNavigationsOwnedQueryFixtureBase<TTestStore>, new()
    {
        protected ComplexNavigationsOwnedQueryTestBase(TFixture fixture)
            : base(fixture)
        {
        }

        [ConditionalFact(Skip = "issue #8216")]
        public override void Query_source_materialization_bug_4547()
        {
            base.Query_source_materialization_bug_4547();
        }

        [ConditionalFact(Skip = "issue #8216")]
        public override void Select_join_with_key_selector_being_a_subquery()
        {
            base.Select_join_with_key_selector_being_a_subquery();
        }

        [ConditionalFact(Skip = "issue #8248")]
        public override void Required_navigation_on_a_subquery_with_First_in_projection()
        {
            base.Required_navigation_on_a_subquery_with_First_in_projection();
        }

        // Naked instances not supported
        public override void Key_equality_two_conditions_on_same_navigation()
        {
        }

        // #8172 - One-to-many not supported yet
        public override void Multiple_SelectMany_with_string_based_Include()
        {
        }

        public override void Where_navigation_property_to_collection_of_original_entity_type()
        {
        }

        public override void SelectMany_with_Include1()
        {
        }

        public override void SelectMany_with_Include2()
        {
        }

        public override void Navigations_compared_to_each_other1()
        {
        }

        public override void Navigations_compared_to_each_other2()
        {
        }

        public override void Navigations_compared_to_each_other3()
        {
        }

        public override void Navigations_compared_to_each_other4()
        {
        }

        public override void Navigations_compared_to_each_other5()
        {
        }

        public override void Navigation_with_same_navigation_compared_to_null()
        {
        }

        public override void Multi_level_navigation_compared_to_null()
        {
        }

        public override void Multi_level_navigation_with_same_navigation_compared_to_null()
        {
        }

        public override void Multi_level_include_correct_PK_is_chosen_as_the_join_predicate_for_queries_that_join_same_table_multiple_times()
        {
        }

        public override void Required_navigation_with_Include_ThenInclude()
        {
        }

        public override void SelectMany_nested_navigation_property_required()
        {
        }

        public override void Multiple_include_with_multiple_optional_navigations()
        {
        }

        public override void Multiple_SelectMany_calls()
        {
        }

        public override void Multiple_complex_includes()
        {
        }

        public override void SelectMany_with_navigation_filter_and_explicit_DefaultIfEmpty()
        {
        }

        public override void SelectMany_with_string_based_Include2()
        {
        }

        public override void SelectMany_with_nested_navigation_and_explicit_DefaultIfEmpty()
        {
        }

        public override void Contains_with_subquery_optional_navigation_and_constant_item()
        {
        }

        public override void Include_with_groupjoin_skip_and_take()
        {
        }

        public override void SelectMany_navigation_property_and_projection()
        {
        }

        public override void SelectMany_nested_navigation_property_optional_and_projection()
        {
        }

        public override void Multi_level_include_one_to_many_optional_and_one_to_many_optional_produces_valid_sql()
        {
        }

        public override void SelectMany_navigation_property_with_another_navigation_in_subquery()
        {
        }

        public override void SelectMany_with_string_based_Include1()
        {
        }

        public override void SelectMany_with_navigation_and_explicit_DefaultIfEmpty()
        {
        }

        public override void SelectMany_navigation_property()
        {
        }

        public override void Complex_multi_include_with_order_by_and_paging()
        {
        }

        public override void Select_nav_prop_collection_one_to_many_required()
        {
        }

        public override void Data_reader_is_closed_correct_number_of_times_for_include_queries_on_optional_navigations()
        {
        }

        public override void SelectMany_where_with_subquery()
        {
        }

        public override void Required_navigation_with_Include()
        {
        }

        public override void Complex_multi_include_with_order_by_and_paging_joins_on_correct_key()
        {
        }

        public override void Complex_query_with_optional_navigations_and_client_side_evaluation()
        {
        }

        public override void SelectMany_navigation_property_and_filter_before()
        {
        }

        public override void Multiple_complex_include_select()
        {
        }

        public override void SelectMany_with_navigation_and_Distinct()
        {
        }

        public override void Optional_navigation_with_Include_ThenInclude()
        {
        }

        public override void SelectMany_with_nested_navigation_filter_and_explicit_DefaultIfEmpty()
        {
        }

        public override void SelectMany_with_Include_ThenInclude()
        {
        }

        public override void Include_nested_with_optional_navigation()
        {
        }

        public override void Multiple_SelectMany_with_navigation_and_explicit_DefaultIfEmpty()
        {
        }

        public override void Multiple_optional_navigation_with_Include()
        {
        }

        public override void Where_navigation_property_to_collection()
        {
        }

        public override void Multiple_SelectMany_with_Include()
        {
        }

        public override void Multiple_optional_navigation_with_string_based_Include()
        {
        }

        public override void Where_navigation_property_to_collection2()
        {
        }

        public override void Where_on_multilevel_reference_in_subquery_with_outer_projection()
        {
        }

        public override void SelectMany_navigation_property_and_filter_after()
        {
        }

        public override void Complex_multi_include_with_order_by_and_paging_joins_on_correct_key2()
        {
        }

        public override void SelectMany_with_navigation_filter_paging_and_explicit_DefaultIfEmpty()
        {
        }

        public override void Comparing_collection_navigation_on_optional_reference_to_null()
        {
        }

        // Self-ref not supported
        public override void Join_navigation_translated_to_subquery_self_ref()
        {
        }

        public override void Multiple_complex_includes_self_ref()
        {
        }

        public override void Join_condition_optimizations_applied_correctly_when_anonymous_type_with_multiple_properties()
        {
        }

        public override void Multi_level_include_reads_key_values_from_data_reader_rather_than_incorrect_reader_deep_into_the_stack()
        {
        }

        public override void Join_condition_optimizations_applied_correctly_when_anonymous_type_with_single_property()
        {
        }

        public override void Navigation_filter_navigation_grouping_ordering_by_group_key()
        {
        }

        public override void Manually_created_left_join_propagates_nullability_to_navigations()
        {
        }

        public override void Optional_navigation_propagates_nullability_to_manually_created_left_join1()
        {
        }
        public override void Optional_navigation_propagates_nullability_to_manually_created_left_join2()
        {
        }

        public override void GroupJoin_with_complex_subquery_with_joins_does_not_get_flattened()
        {
        }

        public override void GroupJoin_with_complex_subquery_with_joins_does_not_get_flattened2()
        {
        }

        public override void GroupJoin_with_complex_subquery_with_joins_does_not_get_flattened3()
        {
        }

        protected override IQueryable<Level1> GetExpectedLevelOne()
            => ComplexNavigationsData.SplitLevelOnes.AsQueryable();

        protected override IQueryable<Level2> GetExpectedLevelTwo()
            => GetExpectedLevelOne().Select(t => t.OneToOne_Required_PK).Where(t => t != null);

        protected override IQueryable<Level3> GetExpectedLevelThree()
            => GetExpectedLevelTwo().Select(t => t.OneToOne_Required_PK).Where(t => t != null);

        protected override IQueryable<Level4> GetExpectedLevelFour()
            => GetExpectedLevelThree().Select(t => t.OneToOne_Required_PK).Where(t => t != null);

        protected override IQueryable<Level2> GetLevelTwo(ComplexNavigationsContext context)
            => GetLevelOne(context).Select(t => t.OneToOne_Required_PK).Where(t => t != null);

        protected override IQueryable<Level3> GetLevelThree(ComplexNavigationsContext context)
            => GetLevelTwo(context).Select(t => t.OneToOne_Required_PK).Where(t => t != null);

        protected override IQueryable<Level4> GetLevelFour(ComplexNavigationsContext context)
            => GetLevelThree(context).Select(t => t.OneToOne_Required_PK).Where(t => t != null);
    }
}
