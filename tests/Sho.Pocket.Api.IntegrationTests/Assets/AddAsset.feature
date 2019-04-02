Feature: AddAsset
	As a user
	I want to add new asset

Background:
	Given currency USD exists

Scenario: User adds new active asset
	Given I specified asset name New active asset, currency USD, is active true
	When I add the asset
	Then asset created
		And asset created with name New active asset
		And asset created with currency USD
		And asset created with is active true

Scenario: User adds new inactive asset
	Given I specified asset name New inactive asset, currency USD, is active false
	When I add the asset
	Then asset created
		And asset created with name New inactive asset
		And asset created with currency USD
		And asset created with is active false
