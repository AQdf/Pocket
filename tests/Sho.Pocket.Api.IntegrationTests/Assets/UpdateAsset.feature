Feature: UpdateAsset
	As a user
	I want to update my asset

Scenario: User updates asset
	Given asset with name Bank account exists in the storage
	When I update asset name to Cash
	Then asset name updated to Cash
