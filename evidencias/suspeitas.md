# Tabela de suspeitas (Fase 1)

| # | Entrada | Obtido | Esperado | Regra |
|---|---|---|---|---|
| 1 | POST analisar 2,3,4 | angulos.c = 100.81 (soma 176.34) | angulos.c = 104.48 | RN06 |
| 2 | GET classificar 5,3,5 | Escaleno | Isosceles | RN03 |
| 3 | GET classificar 3,5,3 | Escaleno | Isosceles | RN03 |
| 4 | GET classificar 1,2,3 | Escaleno | NaoEhTriangulo | RN02 |
| 5 | GET classificar 3,1,2 | Escaleno | NaoEhTriangulo | RN02 |
| 6 | GET classificar 2,3,1 | Escaleno | NaoEhTriangulo | RN02 |
| 7 | GET classificar 10,1,2 | Escaleno | NaoEhTriangulo | RN02 |
| 8 | GET classificar a=3&b=4 (sem c) | 200 NaoEhTriangulo | 400 | RN10 |
| 9 | POST analisar 1,2,3 | 200 com análise | 422 {"erro":"..."} | RN09 / RN10 |
| 10 | GET classificar NaN,1,1 | Isosceles | NaoEhTriangulo | RN01 |
| 11 | GET classificar Infinity,1,1 | Isosceles | NaoEhTriangulo | RN01 / RN02 |