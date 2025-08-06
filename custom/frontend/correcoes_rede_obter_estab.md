# Correções para Problema de Rede - ObterEstab

## Diagnóstico
- API está funcionando (39 registros retornados)
- Problema está na conectividade de rede
- Possível firewall ou proxy bloqueando a conexão

## Correções Necessárias

### 1. Corrigir BaseService.cs
**Arquivo:** `ColetorA41/Services/BaseService.cs`

**Problema:** Timeout infinito está causando travamento
```csharp
// LINHA 32 - MUDANÇA NECESSÁRIA
_httpClient.Timeout = Timeout.InfiniteTimeSpan; // ❌ PROBLEMA
```

**Correção:**
```csharp
// LINHA 32 - CORREÇÃO
_httpClient.Timeout = TimeSpan.FromSeconds(30); // ✅ CORREÇÃO
```

### 2. Melhorar Tratamento de Erro no CalculoViewModel.cs
**Arquivo:** `ColetorA41/ViewModel/CalculoViewModel.cs`

**Problema:** Método ObterEstabelecimentos não tem tratamento adequado para problemas de rede

**Correção - Substituir método atual (linha 1174):**
```csharp
public async Task ObterEstabelecimentos()
{
    try
    {
        this.IsBusy = true;
        
        // Adicionar timeout para evitar travamento
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        
        var lista = await _service.ObterEstabelecimentos().WaitAsync(cts.Token);
        
        if (lista == null || !lista.Any())
        {
            var erro = new Mensagem("erro", "Erro Carregamento", "Não foi possível carregar os estabelecimentos. Verifique sua conexão de rede.");
            await Shell.Current.CurrentPage.ShowPopupAsync(erro);
            return;
        }

        this.listaEstab.Clear();
        foreach (var item in lista.OrderBy(x => x.identific))
        {
            this.listaEstab.Add(item);
        }
    }
    catch (OperationCanceledException)
    {
        var erro = new Mensagem("erro", "Timeout de Rede", "A requisição demorou muito. Verifique sua conexão de rede.");
        await Shell.Current.CurrentPage.ShowPopupAsync(erro);
    }
    catch (Exception ex)
    {
        var erro = new Mensagem("erro", "Erro de Rede", $"Erro ao carregar estabelecimentos: {ex.Message}");
        await Shell.Current.CurrentPage.ShowPopupAsync(erro);
    }
    finally
    {
        this.IsBusy = false;
    }
}
```

### 3. Melhorar BaseService.cs - Método GetAsync
**Arquivo:** `ColetorA41/Services/BaseService.cs`

**Problema:** Falta timeout específico e melhor tratamento de erro

**Correção - Substituir método GetAsync (linha 35):**
```csharp
protected async Task<T?> GetAsync<T>(string endpoint, NameValueCollection parameters = null)
{
    var stringParam = new StringBuilder();

    if (!IsInternetAvailable())
    {
        return default;
    }

    if (parameters != null)
    {
        string str = "?";
        for (int index = 0; index < parameters.Count; ++index)
        {
            stringParam.Append(str + parameters.AllKeys[index] + "=" + parameters[index]);
            str = "&";
        }
    }

    try
    {
        // CORREÇÃO: Adicionar timeout específico para esta requisição
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(25));
        
        var response = await _httpClient.GetAsync(endpoint + stringParam.ToString(), cts.Token);
        
        if (!response.IsSuccessStatusCode)
        {
            Debug.WriteLine($"HTTP Error: {response.StatusCode} - {response.ReasonPhrase}");
            throw new Exception($"Erro HTTP: {response.StatusCode} - {response.ReasonPhrase}");
        }
        
        var responseStream = await response.Content.ReadAsStringAsync();
        
        if (string.IsNullOrEmpty(responseStream))
        {
            Debug.WriteLine("Resposta vazia da API");
            return default;
        }
        
        var data = JsonConvert.DeserializeObject<T>(responseStream);
        return data;
    }
    catch (OperationCanceledException)
    {
        Debug.WriteLine("Timeout na requisição HTTP");
        throw new Exception("Timeout: A requisição demorou muito para responder.");
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"Erro na requisição HTTP: {ex.Message}");
        throw new Exception($"Erro na comunicação com o servidor: {ex.Message}");
    }
}
```

### 4. Configurações de Rede Recomendadas

**Para ambientes corporativos com firewall:**

1. **Verificar se a porta 8243 está liberada**
2. **Verificar se o domínio `hawebdev.dieboldnixdorf.com.br` está liberado**
3. **Configurar proxy se necessário**

**Adicionar configuração de proxy no BaseService:**
```csharp
// No construtor do BaseService, adicionar:
if (!string.IsNullOrEmpty(_config["PROXY_URL"]))
{
    var proxy = new WebProxy(_config["PROXY_URL"]);
    var handler = new HttpClientHandler { Proxy = proxy };
    _httpClient = new HttpClient(handler);
}
```

### 5. Arquivo de Configuração Adicional
**Criar:** `ColetorA41/appsettings.rede.json`
```json
{
    "BASE_URL": "https://hawebdev.dieboldnixdorf.com.br:8243/api/integracao/aat/v1/",
    "USUARIO_SENHA_BASE64": "c3VwZXI6cHJvZGllYm9sZDEx",
    "ALIAS_APPSERVER": "interfcol",
    "EMPRESA_PADRAO": "1",
    "PROXY_URL": "",
    "TIMEOUT_SEGUNDOS": "30"
}
```

## Resumo das Correções
1. ✅ Corrigir timeout infinito no BaseService
2. ✅ Adicionar timeout específico nas requisições
3. ✅ Melhorar tratamento de erro para problemas de rede
4. ✅ Adicionar mensagens específicas para problemas de conectividade
5. ✅ Configurar proxy se necessário

Essas correções devem resolver o problema de travamento do campo ObterEstab em redes corporativas. 