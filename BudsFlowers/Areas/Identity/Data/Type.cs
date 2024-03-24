namespace BudsFlowers.Areas.Identity.Data
{
    public enum TypeStatus
    {
        Не_опубликовано,
        Опубликовано,
        None
    }
    public enum TypeCategory
    {
        Цветы,
        Игрушки,
        Конфеты,
        Другое,
        None
    }
    public enum TypeOrderStatus
    {
        Собирается,
        Доставлено,
        Отказ,
        None
    }
    public enum TypeReview
    {
        Flower,
        All,
        None
    }
    public enum TypeCarousel
    {
        Popular,
        New,
        Sale
    }
    public enum TypeSort
    {
        По_умолчанию,
        Сначала_дешевле,
        Сначала_дороже
    }
}
