Feature: UpdateAsset
	As a user
	I want to update my asset

Background:
	Given currency USD exists

Scenario: User updates asset
	Given I have active asset Updating asset with currency USD
		And I set asset name to Updated asset, is active false
	When I update asset
	Then asset name updated to Updated asset
		And asset is active flag updated to false
