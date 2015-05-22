namespace ACE.Demo.Contracts.Events
open System
open ACE

type AccountAmountChanged = { AccountId: int; Change: decimal } interface IEvent
type AccountStatusCreated = { AccountId: int } interface IEvent
type InvestmentStatusCompleted = { InvestmentId: int } interface IEvent
type InvestmentStatusCreated = { InvestmentId: int; AccountId: int; ProjectId: int; Amount: decimal } interface IEvent
type ProjectAmountChanged = { ProjectId: int; Change: decimal } interface IEvent
