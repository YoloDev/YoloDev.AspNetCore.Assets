if ! [[ "$APPVEYOR_PULL_REQUEST_NUMBER" == "" ]]; then
  git checkout -B "pr-$APPVEYOR_PULL_REQUEST_NUMBER"
else
  git checkout -B "$APPVEYOR_REPO_BRANCH"
fi

if [[ "$(git rev-parse --abbrev-ref HEAD)" != "master" ]]; then
  git fetch origin master:master
else
  echo "git rev-parse --abbrev-ref HEAD:"
  git rev-parse --abbrev-ref HEAD
fi

pushd sample/SampleApp
npm install
popd

DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
VERSION=$("$DIR/git-version.sh" get)
appveyor UpdateBuild -Version "$VERSION ($APPVEYOR_BUILD_NUMBER)"
