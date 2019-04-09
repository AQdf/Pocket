Feature: UpdateBalance
	As a user
	I want to update my balance

Background:
	Given currency USD exists
	And exchange rate USD to USD with value 1, day shift 0
	And I have active asset Active asset with currency USD

Scenario: User updates balance
	Given I have balance of asset Active asset, amount 200, day shift 0
		And I set balance value to 1000
	When I update balance for today of Active asset
	Then balance amount updated to 1000
