export interface currencyTask{
  id?:number,
  name:string,
  description:string,
  userId:number,
  cronExpression:string,
  from:string,
  to:string,
  count?:string
};
