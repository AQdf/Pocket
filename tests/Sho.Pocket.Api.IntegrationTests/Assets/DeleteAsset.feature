Feature: DeleteAsset
	As a user
	I want to delete my asset

Scenario: User deletes asset that is not associated with any balance
	Given asset to delete with name Bank account exists in the storage
	When I delete asset
	Then asset deleted

Scenario: User deletes asset that is associated with balance
	Given asset to delete with name Bank account exists in the storage
	And balance of asset exists in the storage
	When I delete asset
	Then asset not deleted
