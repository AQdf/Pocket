Feature: DeleteBalance
	As a user
	I want to delete my balance

Background:
	Given currency USD exists
	And exchange rate USD to USD with value 1, day shift 0
	And I have active asset Active asset with currency USD

Scenario: User deletes balance
	Given I have balance of asset Active asset, amount 200, day shift 0
		And I specified balance to delete of asset Active asset
	When I delete balance
	Then balance deleted
