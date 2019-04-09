Feature: GetBalances
	As a user
	I want to get my today balances

Background:
	Given currency USD exists
	And exchange rate USD to USD with value 1, day shift 0
	And I have active asset First Asset with balance with currency USD
	And I have active asset Second Asset with balance with currency USD

Scenario: User gets today balances
	Given I have balance of asset First Asset with balance, amount 100, day shift 0
		And I have balance of asset Second Asset with balance, amount 200, day shift 0
	When I get today balances
	Then my 2 balances returned
