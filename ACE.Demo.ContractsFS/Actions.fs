namespace ACE.Demo.Contracts.Actions
open System
open ACE

type InvestmentActionBase = inherit IAction   
type InvestmentCreateRequest = { InvestmentId: int; AccountId: int; ProjectId: int; Amount: decimal } interface InvestmentActionBase
type InvestmentPayRequest = { InvestmentId: int } interface InvestmentActionBase
