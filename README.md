Crypto
======

CryptoAPI library for using PEM key format in .Net apps.
Based on Christian Etter's CryptoAPI wrapper.


Usage
======

Pretty simple -- instantiate a `Crypto.Signer` or `Crypto.Verifier` and invoke their `Sign` and `Verify` methods.
All inputs and signature outputs are handled as strings.
