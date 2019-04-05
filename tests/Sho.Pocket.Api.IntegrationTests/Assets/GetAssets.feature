Feature: GetAssets
	As a user
	I want to get my assets

Background: 
	Given currency USD exists

Scenario: User gets assets
	Given I have active asset My first asset with currency USD
		And I have active asset My second asset with currency USD
	When I get assets
	Then my 2 assets returned
