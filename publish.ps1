param
(
    [string]$name,[string]$uploadurl
)

$tag = get-date -format "yyyyMMdd.HHmm"
$upload = ($uploadurl + "/" + $name + ":" + $tag);

docker build -t $name .
docker tag ($name + ":latest") $upload
docker push $upload