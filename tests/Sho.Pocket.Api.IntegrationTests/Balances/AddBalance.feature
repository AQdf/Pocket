Feature: AddBalance
	As a user
	I want to add new balance of asset

Background:
	Given currency USD exists
	And exchange rate USD to USD with value 1, day shift 0
	And I have active asset Active asset with currency USD

Scenario: User adds new balance of asset
	Given I specified balance of asset Active asset, amount 200, day shift 0
	When I add new balance
	Then balance exists
		And balance asset is Active asset
		And balance amount is 200
		And balance of Active asset effective date is today
