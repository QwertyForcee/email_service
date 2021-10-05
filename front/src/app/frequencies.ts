export const Frequencies=[
  ['1 time a day',{index:2,value:1}],
  ['1 time in 2 days',{index:2,value:2}],
  ['every hour',{index:0,value:0}],
  ['weekly on Sunday',{index:4,value:0}],
  ['weekly on Monday',{index:4,value:1}],
  ['weekly on Tuesday',{index:4,value:2}],
  ['weekly on Wednesday',{index:4,value:3}],
  ['weekly on Thursday',{index:4,value:4}],
  ['weekly on Friday',{index:4,value:5}],
  ['weekly on Saturday',{index:4,value:6}],
  //['every 2 hours',{index:0,value:0}],
  //['every 3 hours',{index:0,value:0}],
]
export function ToCronExpr(Frequency:any,ExecutionMoment:string){
  let minutes:number =  +ExecutionMoment.substring(3)
  let hours:number = +ExecutionMoment.substring(0,2)

  let exp = ['*','*','*','*','*']

  exp[0] = minutes.toString()
  exp[1] = hours.toString()
  if (Frequency.index===0){
    exp[1]= '*'
  }
  else if (Frequency.index===4){
    exp[4] = Frequency.value
  }
  //exp[Frequency.index] = exp[Frequency.index]+'/'+Frequency.value
  let res=  exp.join(' ')
  console.log(Frequency)
  return res
}
