{
  description = "Super mario oddyssey Online server";
  inputs = {
    stable.url = "github:nixos/nixpkgs/nixos-24.05";
    nixpkgs.url = "github:nixos/nixpkgs/nixpkgs-unstable";
    flake-utils.url = "github:numtide/flake-utils/main";
  };
  outputs =
    {
      self,
      nixpkgs,
      stable,
      flake-utils,
    }:
    flake-utils.lib.eachDefaultSystem (
      system:
      let
        pkgs = nixpkgs.legacyPackages.${system};
        stablePkgs = stable.legacyPackages.${system};
      in
      rec {
        packages = flake-utils.lib.flattenTree rec {
          smoOnline = pkgs.buildDotnetModule {
            pname = "SmoOnlineServer";
            version = "1.0.4";

            src = ./.;
            nugetDeps = ./deps.nix;
            packNupkg = false;
            projectFile = "./Server/Server.csproj";
            dotnet-sdk = pkgs.dotnetCorePackages.sdk_6_0;
            dotnet-runtime = pkgs.dotnetCorePackages.runtime_6_0;
            buildType = "Release";
            executables = [ "Server" ];
          };
          default = smoOnline;
        };
        devShell = pkgs.mkShell {
          nativeBuildInputs = [ ];
          buildInputs = with pkgs; [
            pkgs.dotnetCorePackages.sdk_6_0
            pkgs.nuget-to-nix
          ];
        };
      }
    );
}
