open System.Numerics

// Compute the binomial coefficient
let binomialCoefficient n k =
    let rec fact x = if x <= 1I then 1I else x * fact (x - 1I)
    fact n / (fact k * fact (n - k))

// Compute Bernoulli numbers using the Akiyama-Tanigawa algorithm
let bernoulliNumbers n =
    let a = Array.init (n + 1) (fun _ -> BigInteger.Zero)
    let b = Array.init (n + 1) (fun i -> BigInteger.One / BigInteger(i + 1))
    for m = 1 to n do
        for j = m downto 1 do
            b.[j-1] <- BigInteger(j) * (b.[j-1] - b.[j])
    Array.sub b 0 (n + 1)

let faulhaber n p =
    let bernoulli = bernoulliNumbers (p + 1)
    let sum = 
        [for j in 0 .. p do
            let binom = binomialCoefficient (BigInteger(p + 1)) (BigInteger(j))
            let term = binom * bernoulli.[j] * BigInteger.Pow(BigInteger(n), p + 1 - j)
            yield term]
        |> List.sum
    sum / BigInteger(p + 1)

// Example usage
let n = 10
let p = 3
printfn "Sum of the first %d numbers raised to the power of %d is %A" n p (faulhaber n p)
