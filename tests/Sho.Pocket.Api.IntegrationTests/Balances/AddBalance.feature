Feature: AddBalance
	As a user
	I want to add new balance of asset

Background:
	Given currency USD exists
	And exchange rate USD to USD with value 1 for today
	And I have active asset Active asset with currency USD

Scenario: User adds new balance of asset
	Given I specified today balance of asset Active asset, amount 200
	When I add new balance
	Then balance exists
		And balance asset is Active asset
		And balance amount is 200
		And balance effective date is today
