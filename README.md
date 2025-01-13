# .NET Observability ve Clean Architecture Projesi

Bu proje, genel olarak **Observability** kavramını test etmek amacıyla geliştirilmiştir. Bu süreçte farklı özellikler üzerinde de geliştirmeler yapılmıştır.

## Genel Özellikler

- **Minimal API Yaklaşımı:** Projede Controller yerine **MinimalAPI** yaklaşımı uygulanmıştır. Endpoint'ler, `IEndpoint` isimli bir extension aracılığıyla otomatik olarak inject edilmekte ve uygulamaya register edilmektedir.
- **API Versioning:** Endpoint'ler için API versioning kullanılmaktadır.
- **Veritabanı:** MSSQL kullanılmış olup, şu anda yalnızca `users` tablosu bulunmaktadır. Bu tabloda kullanıcı kayıt işlemleri yapılmaktadır.
- **CQRS Design Pattern:** Veritabanı ile API arasındaki işlemler için CQRS design pattern, Mediatr kütüphanesi kullanılarak uygulanmıştır.
- **Clean Architecture ve Vertical Slice:** Tüm projeler **Clean architecture** ve **vertical slice architecture** prensiplerine göre tasarlanmıştır.
- **Exception Handling:** Custom exception handling, Problem Details ile entegre edilerek uygulanmıştır. Kullanıcılara dönen tüm hata mesajları Problem Details formatında sunulmaktadır.
- **Loglama:** Loglama işlemleri için **Serilog** kullanılmaktadır.
- **Docker Üzerinde Monitoring:** Docker üzerinde **Seq** ve **Jaeger** container'ları çalıştırılmaktadır.
- **ORM:** ORM aracı olarak **EFCore** kullanılmaktadır.

## Kurulum ve Çalıştırma

**Gereksinimler:**
   - [.NET SDK](https://dotnet.microsoft.com/download)
   - [Docker](https://www.docker.com/get-started)
   - MSSQL Server

## Kullanılan Teknolojiler

- .NET
- OpenTelemetry
- Mediatr
- Serilog
- Jaeger
- Seq
- Docker
- MSSQL
- EFCore

## Katkıda Bulunma

Katkıda bulunmak isteyenler için pull requestler açıktır. Lütfen değişiklik yapmadan önce bir issue açın.

## Lisans

Bu proje MIT Lisansı ile lisanslanmıştır.
