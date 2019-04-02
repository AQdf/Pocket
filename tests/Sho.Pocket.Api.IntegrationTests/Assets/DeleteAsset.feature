Feature: DeleteAsset
	As a user
	I want to delete my asset

Background: 
	Given currency USD exists

Scenario: User deletes asset that is not associated with any balance
	Given I have active asset Asset without balance with currency USD
	And I specified asset to delete Asset without balance
	When I delete asset Asset without balance
	Then asset deleted

Scenario: User deletes asset that is associated with balance
	Given I have active asset Asset with balance with currency USD
	And I specified asset to delete Asset with balance
		And balance of asset exists in the storage
	When I delete asset Asset with balance
	Then asset not deleted
