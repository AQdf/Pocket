Feature: UpdateAsset
	As a user
	I want to update my asset

Background:
	Given currency USD exists

Scenario: User updates asset
	Given I have active asset Asset to update with currency USD
		And I set asset name to Updated asset, currency USD, is active false
	When I update asset Asset to update
	Then asset name updated to Updated asset
		And asset is active flag updated to false
