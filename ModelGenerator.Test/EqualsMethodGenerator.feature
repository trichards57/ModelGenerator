Feature: Generating class equals methods
	Equals method compare all existing properties it except navigation properties

Scenario: A Create view model clone method is created
	Given I have the test class
		| Name |
		| T1   |
	And it has the properties
		| Name | Type | IncludeInUpdate | IncludeInDetail | IncludeInSummary | IncludeInCreate | GenerateAsList | NavigationPropertyId |
		| N1   | T1   | true            | false           | false            | false           | true           |                      |
		| N2   | T2   | true            | false           | false            | false           | false          |                      |
		| N3   | T3   | false           | true            | false            | false           | true           |                      |
		| N4   | T4   | false           | true            | false            | false           | false          |                      |
		| N5   | T5   | false           | false           | true             | false           | true           |                      |
		| N6   | T6   | false           | false           | true             | false           | false          |                      |
		| N7   | T5   | false           | false           | false            | true            | true           |                      |
		| N8   | T6   | false           | false           | false            | true            | false          |                      |
		| N9   | T2   | true            | false           | false            | false           | false          | P1                   |
		| N10  | T4   | false           | true            | false            | false           | false          | P2                   |
		| N11  | T6   | false           | false           | true             | false           | false          | P3                   |
		| N12  | T6   | false           | false           | false            | true            | false          | P4                   |
	And the function generator is in Create mode
	When I generate an equals method
	Then there should be a public function bool Equals(object other)
	And it should compare properties N8 to item
	And it should not assign properties N1,N2,N3,N4,N5,N6,N7,N9,N10,N11,N12 to item

Scenario: A Model clone method is created
	Given I have the test class
		| Name |
		| T1   |
	And it has the properties
		| Name | Type | IncludeInUpdate | IncludeInDetail | IncludeInSummary | IncludeInCreate | GenerateAsList | NavigationPropertyId |
		| N1   | T1   | true            | false           | false            | false           | true           |                      |
		| N2   | T2   | true            | false           | false            | false           | false          |                      |
		| N3   | T3   | false           | true            | false            | false           | true           |                      |
		| N4   | T4   | false           | true            | false            | false           | false          |                      |
		| N5   | T5   | false           | false           | true             | false           | true           |                      |
		| N6   | T6   | false           | false           | true             | false           | false          |                      |
		| N7   | T5   | false           | false           | false            | true            | true           |                      |
		| N8   | T6   | false           | false           | false            | true            | false          |                      |
		| N9   | T2   | true            | false           | false            | false           | false          | P1                   |
		| N10  | T4   | false           | true            | false            | false           | false          | P2                   |
		| N11  | T6   | false           | false           | true             | false           | false          | P3                   |
		| N12  | T6   | false           | false           | false            | true            | false          | P4                   |
	And the function generator is in Model mode
	When I generate a clone method
	Then there should be a public function object Clone()
	And it should assign properties N2,N4,N6,N8 to item
	And it should not assign properties N1,N3,N5,N7,N9,N10,N11,N12 to item

Scenario: A Update view model clone method is created
	Given I have the test class
		| Name |
		| T1   |
	And it has the properties
		| Name | Type | IncludeInUpdate | IncludeInDetail | IncludeInSummary | IncludeInCreate | GenerateAsList | NavigationPropertyId |
		| N1   | T1   | true            | false           | false            | false           | true           |                      |
		| N2   | T2   | true            | false           | false            | false           | false          |                      |
		| N3   | T3   | false           | true            | false            | false           | true           |                      |
		| N4   | T4   | false           | true            | false            | false           | false          |                      |
		| N5   | T5   | false           | false           | true             | false           | true           |                      |
		| N6   | T6   | false           | false           | true             | false           | false          |                      |
		| N7   | T5   | false           | false           | false            | true            | true           |                      |
		| N8   | T6   | false           | false           | false            | true            | false          |                      |
		| N9   | T2   | true            | false           | false            | false           | false          | P1                   |
		| N10  | T4   | false           | true            | false            | false           | false          | P2                   |
		| N11  | T6   | false           | false           | true             | false           | false          | P3                   |
		| N12  | T6   | false           | false           | false            | true            | false          | P4                   |
	And the function generator is in Update mode
	When I generate a clone method
	Then there should be a public function object Clone()
	And it should assign properties N2 to item
	And it should not assign properties N1,N3,N4,N5,N6,N7,N8,N9,N10,N11,N12 to item

Scenario: A Details view model clone method is created
	Given I have the test class
		| Name |
		| T1   |
	And it has the properties
		| Name | Type | IncludeInUpdate | IncludeInDetail | IncludeInSummary | IncludeInCreate | GenerateAsList | NavigationPropertyId |
		| N1   | T1   | true            | false           | false            | false           | true           |                      |
		| N2   | T2   | true            | false           | false            | false           | false          |                      |
		| N3   | T3   | false           | true            | false            | false           | true           |                      |
		| N4   | T4   | false           | true            | false            | false           | false          |                      |
		| N5   | T5   | false           | false           | true             | false           | true           |                      |
		| N6   | T6   | false           | false           | true             | false           | false          |                      |
		| N7   | T5   | false           | false           | false            | true            | true           |                      |
		| N8   | T6   | false           | false           | false            | true            | false          |                      |
		| N9   | T2   | true            | false           | false            | false           | false          | P1                   |
		| N10  | T4   | false           | true            | false            | false           | false          | P2                   |
		| N11  | T6   | false           | false           | true             | false           | false          | P3                   |
		| N12  | T6   | false           | false           | false            | true            | false          | P4                   |
	And the function generator is in Details mode
	When I generate a clone method
	Then there should be a public function object Clone()
	And it should assign properties N4 to item
	And it should not assign properties N1,N2,N3,N5,N6,N7,N8,N9,N10,N11,N12 to item

Scenario: A Summary view model clone method is created
	Given I have the test class
		| Name |
		| T1   |
	And it has the properties
		| Name | Type | IncludeInUpdate | IncludeInDetail | IncludeInSummary | IncludeInCreate | GenerateAsList | NavigationPropertyId |
		| N1   | T1   | true            | false           | false            | false           | true           |                      |
		| N2   | T2   | true            | false           | false            | false           | false          |                      |
		| N3   | T3   | false           | true            | false            | false           | true           |                      |
		| N4   | T4   | false           | true            | false            | false           | false          |                      |
		| N5   | T5   | false           | false           | true             | false           | true           |                      |
		| N6   | T6   | false           | false           | true             | false           | false          |                      |
		| N7   | T5   | false           | false           | false            | true            | true           |                      |
		| N8   | T6   | false           | false           | false            | true            | false          |                      |
		| N9   | T2   | true            | false           | false            | false           | false          | P1                   |
		| N10  | T4   | false           | true            | false            | false           | false          | P2                   |
		| N11  | T6   | false           | false           | true             | false           | false          | P3                   |
		| N12  | T6   | false           | false           | false            | true            | false          | P4                   |
	And the function generator is in Summary mode
	When I generate a clone method
	Then there should be a public function object Clone()
	And it should assign properties N6 to item
	And it should not assign properties N1,N2,N3,N4,N5,N7,N8,N9,N10,N11,N12 to item

Scenario: A Create view model ToItem method is created
	Given I have the test class
		| Name |
		| T1   |
	And it has the properties
		| Name | Type | IncludeInUpdate | IncludeInDetail | IncludeInSummary | IncludeInCreate | GenerateAsList | NavigationPropertyId |
		| N1   | T1   | true            | false           | false            | false           | true           |                      |
		| N2   | T2   | true            | false           | false            | false           | false          |                      |
		| N3   | T3   | false           | true            | false            | false           | true           |                      |
		| N4   | T4   | false           | true            | false            | false           | false          |                      |
		| N5   | T5   | false           | false           | true             | false           | true           |                      |
		| N6   | T6   | false           | false           | true             | false           | false          |                      |
		| N7   | T5   | false           | false           | false            | true            | true           |                      |
		| N8   | T6   | false           | false           | false            | true            | false          |                      |
		| N9   | T2   | true            | false           | false            | false           | false          | P1                   |
		| N10  | T4   | false           | true            | false            | false           | false          | P2                   |
		| N11  | T6   | false           | false           | true             | false           | false          | P3                   |
		| N12  | T6   | false           | false           | false            | true            | false          | P4                   |
	And the function generator is in Create mode
	When I generate a ToItem method
	Then there should be a public function T1 ToItem()
	And it should assign properties N8 to item
	And it should not assign properties N1,N2,N3,N4,N5,N6,N7,N9,N10,N11,N12 to item

Scenario: A Model ToItem method is created
	Given I have the test class
		| Name |
		| T1   |
	And it has the properties
		| Name | Type | IncludeInUpdate | IncludeInDetail | IncludeInSummary | IncludeInCreate | GenerateAsList | NavigationPropertyId |
		| N1   | T1   | true            | false           | false            | false           | true           |                      |
		| N2   | T2   | true            | false           | false            | false           | false          |                      |
		| N3   | T3   | false           | true            | false            | false           | true           |                      |
		| N4   | T4   | false           | true            | false            | false           | false          |                      |
		| N5   | T5   | false           | false           | true             | false           | true           |                      |
		| N6   | T6   | false           | false           | true             | false           | false          |                      |
		| N7   | T5   | false           | false           | false            | true            | true           |                      |
		| N8   | T6   | false           | false           | false            | true            | false          |                      |
		| N9   | T2   | true            | false           | false            | false           | false          | P1                   |
		| N10  | T4   | false           | true            | false            | false           | false          | P2                   |
		| N11  | T6   | false           | false           | true             | false           | false          | P3                   |
		| N12  | T6   | false           | false           | false            | true            | false          | P4                   |
	And the function generator is in Model mode
	When I generate a ToItem method
	Then there should be no output

Scenario: A Update view model ToItem method is created
	Given I have the test class
		| Name |
		| T1   |
	And it has the properties
		| Name | Type | IncludeInUpdate | IncludeInDetail | IncludeInSummary | IncludeInCreate | GenerateAsList | NavigationPropertyId |
		| N1   | T1   | true            | false           | false            | false           | true           |                      |
		| N2   | T2   | true            | false           | false            | false           | false          |                      |
		| N3   | T3   | false           | true            | false            | false           | true           |                      |
		| N4   | T4   | false           | true            | false            | false           | false          |                      |
		| N5   | T5   | false           | false           | true             | false           | true           |                      |
		| N6   | T6   | false           | false           | true             | false           | false          |                      |
		| N7   | T5   | false           | false           | false            | true            | true           |                      |
		| N8   | T6   | false           | false           | false            | true            | false          |                      |
		| N9   | T2   | true            | false           | false            | false           | false          | P1                   |
		| N10  | T4   | false           | true            | false            | false           | false          | P2                   |
		| N11  | T6   | false           | false           | true             | false           | false          | P3                   |
		| N12  | T6   | false           | false           | false            | true            | false          | P4                   |
	And the function generator is in Update mode
	When I generate a ToItem method
	Then there should be no output

Scenario: A Details view model ToItem method is created
	Given I have the test class
		| Name |
		| T1   |
	And it has the properties
		| Name | Type | IncludeInUpdate | IncludeInDetail | IncludeInSummary | IncludeInCreate | GenerateAsList | NavigationPropertyId |
		| N1   | T1   | true            | false           | false            | false           | true           |                      |
		| N2   | T2   | true            | false           | false            | false           | false          |                      |
		| N3   | T3   | false           | true            | false            | false           | true           |                      |
		| N4   | T4   | false           | true            | false            | false           | false          |                      |
		| N5   | T5   | false           | false           | true             | false           | true           |                      |
		| N6   | T6   | false           | false           | true             | false           | false          |                      |
		| N7   | T5   | false           | false           | false            | true            | true           |                      |
		| N8   | T6   | false           | false           | false            | true            | false          |                      |
		| N9   | T2   | true            | false           | false            | false           | false          | P1                   |
		| N10  | T4   | false           | true            | false            | false           | false          | P2                   |
		| N11  | T6   | false           | false           | true             | false           | false          | P3                   |
		| N12  | T6   | false           | false           | false            | true            | false          | P4                   |
	And the function generator is in Details mode
	When I generate a ToItem method
	Then there should be no output

Scenario: A Summary view model ToItem method is created
	Given I have the test class
		| Name |
		| T1   |
	And it has the properties
		| Name | Type | IncludeInUpdate | IncludeInDetail | IncludeInSummary | IncludeInCreate | GenerateAsList | NavigationPropertyId |
		| N1   | T1   | true            | false           | false            | false           | true           |                      |
		| N2   | T2   | true            | false           | false            | false           | false          |                      |
		| N3   | T3   | false           | true            | false            | false           | true           |                      |
		| N4   | T4   | false           | true            | false            | false           | false          |                      |
		| N5   | T5   | false           | false           | true             | false           | true           |                      |
		| N6   | T6   | false           | false           | true             | false           | false          |                      |
		| N7   | T5   | false           | false           | false            | true            | true           |                      |
		| N8   | T6   | false           | false           | false            | true            | false          |                      |
		| N9   | T2   | true            | false           | false            | false           | false          | P1                   |
		| N10  | T4   | false           | true            | false            | false           | false          | P2                   |
		| N11  | T6   | false           | false           | true             | false           | false          | P3                   |
		| N12  | T6   | false           | false           | false            | true            | false          | P4                   |
	And the function generator is in Summary mode
	When I generate a ToItem method
	Then there should be no output
