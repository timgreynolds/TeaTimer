# TeaTimer
I originally created this project as a simple way to teach myself cross-platform development using Xamarin. It was migrated to MAUI when M$oft included MAUI workloads in dotnet.

It illustrates several interesting design patterns/architectural decisions that I wanted to become more familiar with:
* Service Providers and Constructor-based Dependency Injection
* MVC v. MVVM (leveraging Dependency Injection)
* Platform-specific implementations using conditional compilation v. partial classes/methods
* Local data storage and retreival (leveraging MAUI-based implentations) using SQLite
