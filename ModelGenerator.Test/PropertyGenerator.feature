Feature: Generate Property
	A property should be based on the specification in the provided file

Scenario Outline: A basic property is generated
	Given there is a single property
		| Name | Type |
		| N1   | T1   |
	And the generator is in <mode> mode
	When you create a single property
	Then the output should be a basic property

	Examples:
		| mode    |
		| Create  |
		| Details |
		| Model   |
		| Summary |
		| Update  |

Scenario: A model string property is generated
	Given there is a single property
		| Name | Type     |
		| N1   | string   |
	And the generator is in Model mode
	When you create a single property
	Then the output should be a basic property
	And the property should be initialised with an empty string

Scenario Outline: A view model string property is generated
	Given there is a single property
		| Name | Type     |
		| N1   | string   |
	And the generator is in <mode> mode
	When you create a single property
	Then the output should be a basic property
	And the property should not be initialised

	Examples:
		| mode    |
		| Create  |
		| Details |
		| Summary |
		| Update  |

Scenario: A model list property is generated
	Given there is a single property
		| Name | Type | GenerateAsList |
		| N1   | T1   | true           |
	And the generator is in Model mode
	When you create a single property
	Then the output should be a collection property
	And the property should be initialised with a hashset

Scenario Outline: A view model list property is generated
	Given there is a single property
		| Name | Type | GenerateAsList |
		| N1   | T1   | true           |
	And the generator is in <mode> mode
	When you create a single property
	Then the output should be an enumerable <mode> property
	And the property should not be initialised

	Examples:
		| mode    |
		| Create  |
		| Details |
		| Summary |
		| Update  |

Scenario: A model property is marked with a navigation property id
	Given there is a single property
		| Name | Type | NavigationPropertyId |
		| N1   | T1   | PropId               |
	And the generator is in Model mode
	When you create a single property
	Then the output should be a basic property
	And the output should have a ForeignKey("PropId") attribute

Scenario Outline: A view model property is marked with a navigation property id
	Given there is a single property
		| Name | Type | NavigationPropertyId |
		| N1   | T1   | PropId               |
	And the generator is in <mode> mode
	When you create a single property
	Then the output should be a basic property
	And the output should not have a ForeignKey attribute

	Examples:
		| mode    |
		| Create  |
		| Details |
		| Summary |
		| Update  |

Scenario: A model property is marked with a display name
	Given there is a single property
		| Name | Type | DisplayName |
		| N1   | T1   | Display     |
	And the generator is in Model mode
	When you create a single property
	Then the output should be a basic property
	And the output should not have a DisplayName attribute

Scenario Outline: A view model property is marked with a display name
	Given there is a single property
		| Name | Type | DisplayName |
		| N1   | T1   | Display     |
	And the generator is in <mode> mode
	When you create a single property
	Then the output should be a basic property
	And the output should have a Display(Name="Display") attribute

	Examples:
		| mode    |
		| Create  |
		| Details |
		| Summary |
		| Update  |

Scenario Outline: A model or read-only view model property is marked as an email address
	Given there is a single property
		| Name | Type | ValidateAsEmail |
		| N1   | T1   | true            |
	And the generator is in <mode> mode
	When you create a single property
	Then the output should be a basic property
	And the output should not have a EmailAddress attribute

	Examples:
		| mode    |
		| Model   |
		| Details |
		| Summary |

Scenario Outline: A view model property as an email address
	Given there is a single property
		| Name | Type | ValidateAsEmail |
		| N1   | T1   | true            |
	And the generator is in <mode> mode
	When you create a single property
	Then the output should be a basic property
	And the output should have a EmailAddress attribute

	Examples:
		| mode    |
		| Create  |
		| Update  |

Scenario Outline: A model or read-only view model property is marked with a regular expression
	Given there is a single property
		| Name | Type | RegularExpression |
		| N1   | T1   | RegExp            |
	And the generator is in <mode> mode
	When you create a single property
	Then the output should be a basic property
	And the output should not have a RegularExpression attribute

	Examples:
		| mode    |
		| Model   |
		| Details |
		| Summary |

Scenario Outline: A view model property is marked with a regular expression
	Given there is a single property
		| Name | Type | RegularExpression |
		| N1   | T1   | RegExp            |
	And the generator is in <mode> mode
	When you create a single property
	Then the output should be a basic property
	And the output should have a RegularExpression(@"RegExp") attribute

	Examples:
		| mode    |
		| Create  |
		| Update  |

Scenario Outline: A read-only model property is marked as required
	Given there is a single property
		| Name | Type | PropertyRequired |
		| N1   | T1   | true             |
	And the generator is in <mode> mode
	When you create a single property
	Then the output should be a basic property
	And the output should not have a Required attribute

	Examples:
		| mode    |
		| Details |
		| Summary |

Scenario Outline: A writeable model property is marked as required
	Given there is a single property
		| Name | Type | PropertyRequired |
		| N1   | T1   | true             |
	And the generator is in <mode> mode
	When you create a single property
	Then the output should be a basic property
	And the output should have a Required attribute

	Examples:
		| mode    |
		| Model   |
		| Create  |
		| Update  |
