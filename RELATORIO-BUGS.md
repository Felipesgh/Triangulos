# Relatorio de Bugs

**Aluno(s):** Felipe Garcia (563485)  **Turma:** 2TDSA  **Data:** 01/10/2026

## 1. Resumo

| Item | Valor |
|------|-------|
| Total de casos de teste escritos | 196 |
| Casos vermelhos no codigo original ("antes") | 89 |
| Casos vermelhos no codigo corrigido ("depois") | 0 |
| Cobertura de linhas (Triangulos.Core) | [XX]% |
| Cobertura de ramos (Triangulos.Core) | [XX]% |

## 2. Bugs encontrados

| ID | Regra violada (RN) | Sintoma (entrada -> obtido x esperado) | Local (arquivo:metodo) | Causa raiz | Correcao aplicada | Teste que prova (nome) | Commit |
|----|--------------------|----------------------------------------|------------------------|------------|-------------------|------------------------|--------|
| B01 | RN02 | 1,2,3 -> Escaleno x NaoEhTriangulo | ClassificadorTriangulo.cs:EhTriangulo | Comparacao com `<` deixava passar soma igual ao terceiro lado (triangulo degenerado) | `<` trocado por `<=` | Classificar_SomaDeDoisLadosIgualAoTerceiro_RetornaNaoEhTriangulo | ce3084d |
| B02 | RN02 | 10,1,2 -> Escaleno x NaoEhTriangulo | ClassificadorTriangulo.cs:EhTriangulo | Faltava a desigualdade b + c > a (o lado a nunca era validado) | Acrescentado `if (b + c <= a) return false;` | Classificar_SomaDeDoisLadosMenorQueOTerceiro_RetornaNaoEhTriangulo | 265c550 |
| B03 | RN01 | NaN,1,1 -> Isosceles x NaoEhTriangulo; Infinity,1,1 idem | ClassificadorTriangulo.cs:EhTriangulo | NaN e Infinity nao eram rejeitados: com NaN todas as comparacoes sao falsas e o metodo retornava true | Checagem `double.IsFinite` no inicio | Classificar_LadoInvalido_RetornaNaoEhTriangulo | 3b1e020 |
| B04 | RN03 | 3,5,3 -> Escaleno x Isosceles | ClassificadorTriangulo.cs:Classificar | Isosceles comparava so a==b e b==c, faltava a==c | Acrescentado `|| a == c` | Classificar_ExatamenteDoisLadosIguais_RetornaIsosceles | fe8c989 |
| B05 | RN06 | 2,3,4 -> angulo C 100.81 x 104.48 (soma dos angulos 176.34) | CalculadoraTriangulo.cs:Angulos | Denominador do angulo C era 2*a*c (copiado do angulo B) em vez de 2*a*b | Trocado para `2 * a * b` | Angulos_TrianguloValido_RetornaAngulosOpostosAosLados | 1da947f |
| B06 | RN05 | 1,1,1 -> area 0.4 x 0.43 | CalculadoraTriangulo.cs:Area | Math.Round com 1 casa decimal em vez de 2 | `1` trocado por `2` | Area_TrianguloValido_RetornaAreaDeHeronArredondada | bcf828b |
| B07 | RN07/RN08 | 5,3,4 -> Acutangulo x Retangulo | CalculadoraTriangulo.cs:EhRetangulo | Hipotenusa fixa em c; a regra diz que e o maior lado em qualquer posicao | Lados ordenados com Array.Sort; z passa a ser o maior | EhRetangulo_LadosDeTrianguloRetangulo_RetornaTrue | d1cc9d7 |
| B08 | RN10 | GET classificar?a=3&b=4 -> 200 x 400 | Program.cs:/classificar | Parametro c tinha valor padrao 0, entao a ausencia nao gerava erro | `double c = 0` trocado por `double c` | Classificar_ParametroAusenteOuNaoNumerico_Retorna400 | 06e0046 |
| B09 | RN07 | 0.5,1.2,1.3 -> False x True | CalculadoraTriangulo.cs:EhRetangulo | Comparacao com `==` exato, sem tolerancia relativa; 1.3*1.3 vale 1.6900000000000002 | `Math.Abs((x*x + y*y) - z*z) <= 1e-9 * z * z` | EhRetangulo_TrianguloRetanguloComDecimais_RetornaTrue | b65c1fa |

O B03 so aparece com valores especiais de `double` (NaN e Infinity).

## 3. Prova de sensibilidade (pelo menos 3 bugs)

| Bug | Como reintroduzi | Teste que falhou | Evidencia (arquivo em evidencias/sensibilidade/) |
|-----|------------------|------------------|--------------------------------------------------|
| B06 | git revert --no-commit do commit da correcao | Area_TrianguloValido_RetornaAreaDeHeronArredondada | D6-area.txt |
| B05 | git revert --no-commit do commit da correcao | Angulos_TrianguloValido_RetornaAngulosOpostosAosLados | D5-angulo-c.txt |
| B04 | git revert --no-commit do commit da correcao | Classificar_ExatamenteDoisLadosIguais_RetornaIsosceles | D4-isosceles.txt |

## 4. Bugs que eu NAO consegui corrigir / duvidas

Nenhum bug pendente.

## 5. O que aprendi

Aprendi que os testes devem ser criados com base na especificação, e não apenas no comportamento atual do código.
Também aprendi a identificar e corrigir bugs usando testes automatizados, além de comprovar as correções por meio de cobertura e testes de sensibilidade.
Com isso, entendi melhor a importância de manter uma suíte de testes capaz de detectar regressões.
