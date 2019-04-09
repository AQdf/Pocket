Feature: AddEffectiveBalances
	As a user
	I want to add effective balances

Background:
	Given currency USD exists
		And exchange rate USD to USD with value 1, day shift 0
		And exchange rate USD to USD with value 1, day shift -1
	And I have active asset First asset with effective balance with currency USD
	And I have active asset Second asset with effective balance with currency USD

Scenario: User adds effective balances
	Given I have balance of asset First asset with effective balance, amount 100, day shift -1
		And I have balance of asset Second asset with effective balance, amount 200, day shift -1
	When I add effective balances
	Then balances for today exists

