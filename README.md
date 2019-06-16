# Add_ShoppingCart

Bir e-ticaret sitesi için sepete ürün ekleme özelliğini barındıran bir projedir.
Projede aşağıdaki teknoloji ve yazılım pratikleri uygulanmıştır;
* .Net Core 2.1 
* MongoDB
* Katmanlı Mimari 
* Design patterns kullanılarak SOLID prensiplerine uygun kod yazılmaya çalışılmıştır.

## Not
```
MongoDB için kurulum gerekmez. cloud.mongodb.com kullanılmıştır. Harici bir kurulum gerektirmeden Visual studio 2017'de açılıp, çalıştırılabilir.
```

# Proje Yapısı
* Projenin içinde iki controller bulunmaktadır: ___ProductController, ShoppingCartController___
* ProductController içindeki ___AddProduct___ metoduna ürün listesi göndererek ürün eklenir.
* ShoppingCartController içinde iki metoda sahip. 
    * ___CreateShoppingCart___ ilk kez sepet oluşturulurken çağrılacak metottur. İçindeki ürünleri kontrol eder, eğer ürünler veri tabanında varsa ve stok durumları yeterliyse sepete eklenir yoksa hata döner.
    * ___UpdateShoppingCart___ sepeti olan bir kullanıcı sepetine yeni ürün ekledikçe çağrılacak metottur. İçindeki ürünleri kontrol eder, eğer ürünler veri tabanında varsa, stok durumları yeterliyse, son olarak daha önce bu ürünün daha önce sepette olup olmadığı kontrol edildikten sonra(daha önce ürün varsa quantity artırılır) sepete eklenir.

