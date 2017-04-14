Feature: Generating multiple C# model properties
	Multiple properties should be generated based on the provided file

Scenario: The model should get all properties
	Given there are a group of properties
		| Name | Type | IncludeInCreate | IncludeInDetail | IncludeInUpdate | IncludeInSummary |
		| N1   | T1   | false           | false           | false           | false            |
		| N2   | T2   | true            | false           | false           | false            |
		| N3   | T3   | false           | true            | false           | false            |
		| N4   | T4   | false           | false           | true            | false            |
		| N5   | T5   | false           | false           | false           | true             |
		| N6   | T6   | false           | true            | false           | false            |
	And the generator is in Model mode
	When you create a set of properties
	Then the output properties should be N1, N2, N3, N4, N5, N6

Scenario: The create view model should only get IncludeInCreate properties
	Given there are a group of properties
		| Name | Type | IncludeInCreate | IncludeInDetail | IncludeInUpdate | IncludeInSummary |
		| N1   | T1   | false           | false           | false           | false            |
		| N2   | T2   | true            | false           | false           | false            |
		| N3   | T3   | false           | true            | false           | false            |
		| N4   | T4   | false           | false           | true            | false            |
		| N5   | T5   | false           | false           | false           | true             |
		| N6   | T6   | false           | true            | false           | false            |
	And the generator is in Create mode
	When you create a set of properties
	Then the output properties should be N2

Scenario: The detail view model should only get IncludeInDetail properties
	Given there are a group of properties
		| Name | Type | IncludeInCreate | IncludeInDetail | IncludeInUpdate | IncludeInSummary |
		| N1   | T1   | false           | false           | false           | false            |
		| N2   | T2   | true            | false           | false           | false            |
		| N3   | T3   | false           | true            | false           | false            |
		| N4   | T4   | false           | false           | true            | false            |
		| N5   | T5   | false           | false           | false           | true             |
		| N6   | T6   | false           | true            | false           | false            |
	And the generator is in Details mode
	When you create a set of properties
	Then the output properties should be N3, N6

Scenario: The summary view model should only get IncludeInSummary properties
	Given there are a group of properties
		| Name | Type | IncludeInCreate | IncludeInDetail | IncludeInUpdate | IncludeInSummary |
		| N1   | T1   | false           | false           | false           | false            |
		| N2   | T2   | true            | false           | false           | false            |
		| N3   | T3   | false           | true            | false           | false            |
		| N4   | T4   | false           | false           | true            | false            |
		| N5   | T5   | false           | false           | false           | true             |
		| N6   | T6   | false           | true            | false           | false            |
	And the generator is in Summary mode
	When you create a set of properties
	Then the output properties should be N5

Scenario: The update view model should only get IncludeInUpdate properties
	Given there are a group of properties
		| Name | Type | IncludeInCreate | IncludeInDetail | IncludeInUpdate | IncludeInSummary |
		| N1   | T1   | false           | false           | false           | false            |
		| N2   | T2   | true            | false           | false           | false            |
		| N3   | T3   | false           | true            | false           | false            |
		| N4   | T4   | false           | false           | true            | false            |
		| N5   | T5   | false           | false           | false           | true             |
		| N6   | T6   | false           | true            | false           | false            |
	And the generator is in Update mode
	When you create a set of properties
	Then the output properties should be N4
