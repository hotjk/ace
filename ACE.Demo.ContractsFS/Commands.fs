namespace ACE.Demo.Contracts.Commands
open System
open ACE
open ACE.WS

type ChangeAccountAmount = { AccountId: int; Change: decimal } interface ICommand
type ChangeProjectAmount = { ProjectId: int; Change: decimal } interface ICommand
type CompleteInvestment = { InvestmentId: int } interface ICommand
type CreateAccount = { AccountId: int } interface ICommand
type CreateAccountActivity = { FromAccountId: Nullable<int>; ToAccountId: Nullable<int>; Amount: decimal } interface ICommand
type CreateInvestment = { InvestmentId: int; AccountId: int; ProjectId: int; Amount: decimal } interface ICommand
