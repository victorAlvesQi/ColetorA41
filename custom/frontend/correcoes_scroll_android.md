# Correções para Problemas de Scroll no Android

## Problemas Identificados

### 1. ScrollView sem configurações adequadas para Android

**Arquivos afetados:**
- `Views/Monitor/Embalagem.xaml`
- `Views/Calculo/LeituraENC.xaml`

**Problema:** ScrollView não tem configurações específicas para Android que garantam o scroll adequado.

### 2. CollectionView sem ScrollView wrapper

**Arquivos afetados:**
- `Views/Monitor/Reparo.xaml`
- `Views/Monitor/Processos.xaml`
- `Views/Calculo/LeituraPagtos.xaml`
- `Views/Calculo/ResumoDetalhe.xaml`
- `Views/Calculo/ResumoDetalheEntrada.xaml`
- `Views/Calculo/ResumoDetalhePagto.xaml`
- `Views/Calculo/ExtrakitView.xaml`
- `Views/Calculo/EstabTec.xaml`

**Problema:** CollectionView sem ScrollView pode ter problemas de scroll no Android.

### 3. Grid RowDefinitions inadequadas

**Problema:** Grid com RowDefinitions fixas podem cortar conteúdo no Android.

## Correções Necessárias

### Correção 1: ScrollView com configurações Android
```xml
<ScrollView Grid.Row="1" 
            Margin="10,0,10,0" 
            HorizontalOptions="Center"
            VerticalScrollBarVisibility="Always"
            Orientation="Vertical">
    <!-- Conteúdo -->
</ScrollView>
```

### Correção 2: CollectionView com ScrollView wrapper
```xml
<ScrollView Grid.Row="1" 
            VerticalScrollBarVisibility="Always"
            Orientation="Vertical">
    <CollectionView x:Name="lista"
                    ItemsSource="{Binding listaItems}"
                    VerticalOptions="FillAndExpand">
        <!-- ItemTemplate -->
    </CollectionView>
</ScrollView>
```

### Correção 3: Grid RowDefinitions flexíveis
```xml
<Grid RowDefinitions="Auto,*,Auto,Auto">
    <!-- Conteúdo -->
</Grid>
```

## Arquivos que precisam ser corrigidos:

1. **Views/Monitor/Embalagem.xaml** - ScrollView precisa de configurações Android
2. **Views/Calculo/LeituraENC.xaml** - ScrollView precisa de configurações Android
3. **Views/Monitor/Reparo.xaml** - CollectionView precisa de ScrollView wrapper
4. **Views/Monitor/Processos.xaml** - CollectionView precisa de ScrollView wrapper
5. **Views/Calculo/LeituraPagtos.xaml** - CollectionView precisa de ScrollView wrapper
6. **Views/Calculo/ResumoDetalhe.xaml** - CollectionView precisa de ScrollView wrapper
7. **Views/Calculo/ResumoDetalheEntrada.xaml** - CollectionView precisa de ScrollView wrapper
8. **Views/Calculo/ResumoDetalhePagto.xaml** - CollectionView precisa de ScrollView wrapper
9. **Views/Calculo/ExtrakitView.xaml** - CollectionView precisa de ScrollView wrapper
10. **Views/Calculo/EstabTec.xaml** - CollectionView precisa de ScrollView wrapper 