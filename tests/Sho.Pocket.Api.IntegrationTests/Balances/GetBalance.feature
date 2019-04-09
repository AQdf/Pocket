Feature: GetBalance
	As a user
	I want to get my balance of asset

Background:
	Given currency USD exists
	And exchange rate USD to USD with value 1, day shift 0
	And I have active asset Active asset with currency USD

Scenario: User gets balance of asset
	Given I have balance of asset Active asset, amount 200, day shift 0
	When I get balance for today of Active asset
	Then balance exists
		And balance amount is 200
		And balance of Active asset effective date is today
