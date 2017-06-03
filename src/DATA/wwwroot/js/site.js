// Write your Javascript code.


function piechart(d3, dataset, size, tag) {
    var donutWidth = size / 6;

    var width = size;
    var height = size;
    var radius = size / 2;

    var color = d3.scaleOrdinal().range(['#E1004C', '#00A47F', '#FF6200', '#70E500', '#00644E']);

    var svg = d3.select(tag)
      .append('svg')
      .attr('width', width)
      .attr('height', height)
      .append('g')
      .attr('transform', 'translate(' + (width / 2) +
        ',' + (height / 2) + ')');

    var arc = d3.arc()
      .innerRadius(radius - donutWidth)
      .outerRadius(radius);

    var pie = d3.pie()
      .value(function (d) { return d.data; })
      .sort(null);

    var path = svg.selectAll('path')
      .data(pie(dataset))
      .enter()
      .append('path')
      .attr('d', arc)
      .attr('fill', function (d) {
          return color(d.data.title);
      }).append("svg:title")
        .text(function (d) { return d.data.data; });;

    var legendRectSize = size / 16;
    var legendSpacing = size / 24;

    var legend = svg.selectAll('.tweettext')
          .data(color.domain())
          .enter()
          .append('g')
          .attr('transform', function (d, i) {
              var height = legendRectSize + legendSpacing;
              var offset = height * color.domain().length / 2;
              var horz = -2 * legendRectSize;
              var vert = i * height - offset;
              return 'translate(' + horz + ',' + vert + ')';
          });

    legend.append('rect')
          .attr('width', legendRectSize)
          .attr('height', legendRectSize)
          .style('fill', color)
          .style('stroke', color);

    legend.append('text')
          .attr('fill', '#9d9d9d')
          .attr('x', legendRectSize + legendSpacing)
          .attr('y', legendRectSize - legendSpacing+10)
          .text(function (d) { return d; })
          .styles({
              "fill": "#9d9d9d",
              //"fill": "white",
              "font-family": "Coda', cursive",
              "font-size": "12px"
          });

} (window.d3);

function BarGraph(d3, data, size, tag)
{
    var width = size;
    var height = size;

    var margin = {top: 20, right: 20, bottom: 30, left: 40};

    // set the ranges
    var x = d3.scaleBand()
              .range([0, width])
              .padding(0.1);
    var y = d3.scaleLinear()
              .range([height, 0]);
          
    var color = d3.scaleOrdinal().range(['#E1004C', '#00A47F', '#FF6200', '#70E500', '#00644E']);

    var svg = d3.select(tag)
      .append('svg')
      .attr('width', width + margin.left + margin.right)
      .attr('height', height + margin.top + margin.bottom)
      .append('g')
       .attr("transform",
          "translate(" + margin.left + "," + margin.top + ")");

      


        data.forEach(function(d) {
            d.sentiment = +d.sentiment;
        });

        // Scale the range of the data in the domains
        x.domain(data.map(function(d) { return d.title; }));
        y.domain([0, d3.max(data, function(d) { return d.sentiment; })]);

        // append the rectangles for the bar chart
        svg.selectAll(".bar")
            .data(data)
          .enter().append("rect")
            .attr("class", "bar")
            .attr("x", function(d) { return x(d.title); })
            .attr("width", x.bandwidth())
            .attr("y", function(d) { return y(d.sentiment); })
            .attr("height", function(d) { return height - y(d.sentiment); });

        // add the x Axis
        svg.append("g")
            .attr("transform", "translate(0," + height + ")")
            .call(d3.axisBottom(x));

        // add the y Axis
        svg.append("g")
            .call(d3.axisLeft(y));

}

function timegraph(d3, data, size, tag) {

    //$(tag).height(size);
    var margin = { top: 20, right: 20, bottom: 30, left: 50 },
    svg = d3.select(tag).append('svg').attr('height', size).attr('width',$(tag).width()),
    width = $(tag).width() - margin.left - margin.right,
    height = size - margin.top - margin.bottom,
    g = svg.append("g").attr("transform", "translate(" + margin.left + "," + margin.top + ")")
    

    var parseTime = d3.timeParse("%m/%d/%Y %H:%M");

    var x = d3.scaleTime()
        .rangeRound([0, width]);

    var y = d3.scaleLinear()
        .rangeRound([height, 0]);

    var line = d3.line()
        .x(function (d) { return x(d.date); })
        .y(function (d) { return y(d.count); });

    var color = d3.scaleOrdinal().range(['#E1004C', '#00A47F', '#FF6200', '#70E500', '#00644E']);

    data.forEach(function (d) {
        d.date = parseTime(d.date);
        d.count = +d.count;
    }
    );

    x.domain(d3.extent(data, function (d) { return d.date; }));
    y.domain(d3.extent(data, function (d) { return d.count; }));

    g.append("g").attr("stroke", "#9d9d9d")
        .attr("transform", "translate(0," + height + ")")
        .call(d3.axisBottom(x))
        .select(".domain").styles({ stroke: "#9d9d9d" });

    g.append("g").styles({ stroke: "#9d9d9d" })
        .call(d3.axisLeft(y)).select(".domain").styles({ stroke: "#9d9d9d" })
        .append("text")
        .text("# of times Hashtag was used")
        .styles({
            "font-size": "1vh",
            "stroke": "#9d9d9d",
            "text-anchor": "end",
            "dy": "0.71em",
            "y": 6,
            "transform": "rotate(-90)",
            "fill": "#9d9d9d"
        });

    g.append("path")
        .datum(data)
        .attr("fill", "none")
        .attr("stroke", function (d) { return color(d.date); })
        .attr("stroke-linejoin", "round")
        .attr("stroke-linecap", "round")
        .attr("stroke-width", 5)
        .attr("d", line);
}

function pinmap(result,size, tag) {

    var marks = [];
    $.each(result, function (k, v) {
        var mark = { long: v.item1, lat: v.item2 };
        marks.push(mark);
    });

    var height = size,//$(tag).width(),//960,
        width = height * 2;//$(tag).width() / 1.65;//580;
    var color = d3.scaleOrdinal().range(['#E1004C', '#00A47F', '#FF6200', '#70E500', '#00644E']);
    var projection = d3.geo.kavrayskiy7()
        .scale(170)
        .translate([width / 2, height / 2])
        .precision(.1);
    var path = d3.geo.path()
        .projection(projection);
    var graticule = d3.geo.graticule();
    var svg = d3.select(tag).append("svg")
        .attr("width", width)
        .attr("height", height);
    svg.append("defs").append("path")
        .datum({ type: "Sphere" })
        .attr("id", "sphere")
        .attr("d", path);
    svg.append("use")
        .attr("class", "stroke")
        .attr("xlink:href", "#sphere");
    svg.append("use")
    .attr("class", "fill")
    .attr("xlink:href", "#sphere");
    svg.append("path")
        .datum(graticule)
        .attr("class", "graticule")
        .attr("d", path);
    d3.json("https://raw.githubusercontent.com/react-d3/react-d3-map/master/example/data/world-50m.json", function (error, world) {
        if (error) throw error;
        var countries = topojson.feature(world, world.objects.countries).features,
            neighbors = topojson.neighbors(world.objects.countries.geometries);
        var color = d3.scaleOrdinal()
            .domain(countries)
            .range(['#E1004C', '#00A47F', '#FF6200', '#70E500', '#00644E']);
        svg.selectAll(".country")
            .data(countries)
          .enter().insert("path", ".graticule")
            .attr("class", "country")
            .attr("d", path)
            .style("fill", function (d) {
                return color(d.id)
            });
        svg.insert("path", ".graticule")
            .datum(topojson.mesh(world, world.objects.countries, function (a, b) { return a !== b; }))
            .attr("class", "boundary")
            .attr("d", path);




        svg.selectAll(".mark")
            .data(marks)
            .enter()
            .append("image")
            .attr('class', 'mark')
            .attr('width', 20)
            .attr('height', 25)
            .attr("xlink:href", "../../images/locater.svg")
            .attr("transform", function (d) {
                return "translate(" + projection([d.long-6, d.lat+7.5]) + ")";
            });
    });
    d3.select(self.frameElement).style("height", height + "px");


}

function bubblechart(data,size, tag) {
           
    var diameter = size//550, //max size of the bubbles
        color = d3.scaleOrdinal().range(['#E1004C', '#00A47F', '#FF6200', '#70E500', '#00644E']);



    var bubble = d3.layout.pack()
        .sort(null)
        .size([diameter, diameter])
        .padding(1.5);



    var svg = d3.select(tag)
        .append("svg")
            .attr("width", diameter)
            .attr("height", diameter)
            .attr("class", "bubble");


    data = data.map(function (d) { d.value = +d["count"]; return d; });

    //bubbles needs very specific format, convert data to this.
    var nodes = bubble.nodes({ children: data }).filter(function (d) { return !d.children; });


    //setup the chart
    var bubbles = svg.append("g")
        .attr("transform", "translate(0,0)")
        .selectAll(".bubble")
        .data(nodes)
        .enter();


    //create the bubbles
    bubbles.append("circle")
        .attr("r", function (d) { return d.r; })
        .attr("cx", function (d) { return d.x; })
        .attr("cy", function (d) { return d.y; })
        .style("fill", function (d) { return color(d.value); })
        .append("svg:title")
        .text(function (d) { return d["count"]; });

    //format the text for each bubble
    bubbles.append("text")
        .attr("x", function (d) { return d.x; })
        .attr("y", function (d) { return d.y + 5; })
        .attr("text-anchor", "middle")
        .text(function (d) { return d["word"]; })
        .styles({
            "fill": "#9d9d9d" ,
            "font-family": "Coda', cursive",
            "font-size": "14px",
            "font-weight": "bold"
        });


}

function gallery(data,size,tag)
{
    //d3.json(data, function (error, imgs) {

        // Get the modal
        var modal = document.getElementById('myModal');

        // Get the image and insert it inside the modal - use its "alt" text as a caption
        //var img = document.getElementById('myImg');
        var modalImg = document.getElementById("img01");
        var captionText = document.getElementById("caption");

        // Get the <span> element that closes the modal
        var span = document.getElementsByClassName("close")[0];
        $('.close').css('margin-top', $('.container').height());
        modalImg.style.maxWidth = $(window).width();
        // When the user clicks on <span> (x), close the modal
        span.onclick = function () {
            modal.style.display = "none";
        }


        // filter out posts without a thumbnail
        var images = data;//imgs.data.children.filter(function (d) {
            //return d.data.thumbnail.slice(-3) == "jpg";
        //});

        images.forEach(function (img) {
            d3.select(tag)
              .append("img")
              .attr("height", 100)
              .attr("src", img)
              .on("click", function () {
                modal.style.display = "block";
                modalImg.src = this.src;
                captionText.innerHTML = this.alt;
            });
              
        });
    //});
}