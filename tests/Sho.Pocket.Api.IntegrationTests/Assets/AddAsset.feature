Feature: AddAsset
	As a user
	I want to add new asset

Scenario: User adds new asset
	Given asset with name Bank account and currency USD
	When I add the asset
	Then asset created with name Bank account and currency USD
