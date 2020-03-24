Feature: DeleteAsset
	As a user
	I want to delete my asset

Background: 
	Given currency USD exists

Scenario: User deletes asset that is not associated with any balance
	Given I have active asset Asset without balance with currency USD
		And I specified asset to delete Asset without balance
	When I delete asset
	Then asset deleted

Scenario: User deletes asset that is associated with balance
	Given I have active asset Asset with balance with currency USD
		And I have balance of asset Asset with balance, amount 200, day shift 0
		And I specified asset to delete Asset with balance
	When I delete asset
	Then asset not deleted
