Feature: Generating class constructors
	Constructor should only be generated if it's needed
	Constructor should only populated properties that exist
	Constructor should provide a default if needed

Scenario: A Create view model constructor is created
	Given I have the test class
		| Name |
		| T1   |
	And it has the properties
		| Name | Type | IncludeInCreate | GenerateAsList |
		| N1   | T1   | true            | true           |
		| N2   | T2   | true            | false          |
	And the function generator is in Create mode
	When I generate a constructor
	Then there should be no output

Scenario: A Model constructor is created
	Given I have the test class
		| Name |
		| T1   |
	And it has the properties
		| Name | Type | IncludeInCreate | GenerateAsList |
		| N1   | T1   | true            | true           |
		| N2   | T2   | true            | false          |
	And the function generator is in Model mode
	When I generate a constructor
	Then there should be no output

Scenario: A Update view model constructor is created
	Given I have the test class
		| Name |
		| T1   |
	And it has the properties
		| Name | Type | IncludeInUpdate | IncludeInDetail | IncludeInSummary | GenerateAsList |
		| N1   | T1   | true            | false           | false            | true           |
		| N2   | T2   | true            | false           | false            | false          |
		| N3   | T3   | false           | true            | false            | true           |
		| N4   | T4   | false           | true            | false            | false          |
		| N5   | T5   | false           | false           | true             | true           |
		| N6   | T6   | false           | false           | true             | false          |
	And the function generator is in Update mode
	When I generate a constructor
	Then there should be an empty public function T1Update()
	And there should be a public function T1Update(T1 item)
	And it should assign properties N1,N2
	And it should not assign properties N3,N4,N5,N6

Scenario: A Details view model constructor is created
	Given I have the test class
		| Name |
		| T1   |
	And it has the properties
		| Name | Type | IncludeInUpdate | IncludeInDetail | IncludeInSummary | GenerateAsList |
		| N1   | T1   | true            | false           | false            | true           |
		| N2   | T2   | true            | false           | false            | false          |
		| N3   | T3   | false           | true            | false            | true           |
		| N4   | T4   | false           | true            | false            | false          |
		| N5   | T5   | false           | false           | true             | true           |
		| N6   | T6   | false           | false           | true             | false          |
	And the function generator is in Details mode
	When I generate a constructor
	Then there should not be an empty public function T1Details()
	And there should be a public function T1Details(T1 item)
	And it should assign properties N3,N4
	And it should not assign properties N1,N2,N5,N6

Scenario: A Summary view model constructor is created
	Given I have the test class
		| Name |
		| T1   |
	And it has the properties
		| Name | Type | IncludeInUpdate | IncludeInDetail | IncludeInSummary | GenerateAsList |
		| N1   | T1   | true            | false           | false            | true           |
		| N2   | T2   | true            | false           | false            | false          |
		| N3   | T3   | false           | true            | false            | true           |
		| N4   | T4   | false           | true            | false            | false          |
		| N5   | T5   | false           | false           | true             | true           |
		| N6   | T6   | false           | false           | true             | false          |
	And the function generator is in Summary mode
	When I generate a constructor
	Then there should not be an empty public function T1Summary()
	And there should be a public function T1Summary(T1 item)
	And it should assign properties N5,N6
	And it should not assign properties N1,N2,N3,N4
