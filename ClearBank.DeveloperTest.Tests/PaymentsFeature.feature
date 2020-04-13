Feature: PaymentsFeature
	
Background:
	Given I have retrieved debitor account details from the database
	And I have retrieved creditor account details from the database

@unittest
Scenario: Make a payment
	When I have entered 70 debit amount
	And Make a payment request
	Then Verify payment has been made : true
	And Verify debitor account balance after payment

@unittest
Scenario: Payment Fail with invalid PaymentSheme
	When I have entered 70 debit amount
	And Make a payment request with invalid payment scheme
	Then Verify payment has been made : false
	And Verify debitor account balance after payment transaction fail