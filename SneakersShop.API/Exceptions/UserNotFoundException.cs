using System;

namespace SneakersShop.API.Exceptions;

public class UserNotFoundException(string username)
    : Exception($"Korisnik sa korisničkim imenom '{username}' ne postoji.") { }
