// Write your Javascript code.

function piechart(d3, dataset, size, tag) {
    var donutWidth = size / 6;

    var width = size;
    var height = size;
    var radius = size / 2;

    var color = d3.scaleOrdinal()
        .range(['#E1004C', '#00A47F', '#FF6200', '#70E500', '#00644E']);

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
      });

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
          .attr('id', 'tweettext')
          .attr('x', legendRectSize + legendSpacing)
          .attr('y', legendRectSize - legendSpacing)
          .text(function (d) { return d; });

} (window.d3);

function timegraph(d3, data, tag) {

    $(tag).height(window.innerHeight/3);

    var svg = d3.select(tag),
    margin = { top: 20, right: 20, bottom: 30, left: 50 },
    width = $(tag).height()*2 - margin.left - margin.right,
    height = $(tag).height() - margin.top - margin.bottom,
    g = svg.append("g").attr("transform", "translate(" + margin.left + "," + margin.top + ")");

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
        .attr("fill", "#9d9d9d")
        .attr("transform", "rotate(-90)")
        .attr("y", 6)
        .attr("dy", "0.71em")
        .attr("stroke", "#9d9d9d")
        .attr("text-anchor", "end")
        .style("font-size", "1vh")
        .text("# of times Hashtag was used");

    g.append("path")
        .datum(data)
        .attr("fill", "none")
        .attr("stroke", function (d) { return color(d.date); })
        .attr("stroke-linejoin", "round")
        .attr("stroke-linecap", "round")
        .attr("stroke-width", 5)
        .attr("d", line);
}

function pinmap(result, tag) {

    var marks = [];
    $.each(result, function (k, v) {
        var mark = { long: v.item1, lat: v.item2 };
        marks.push(mark);
    });

    var width = $(tag).width(),//960,
     height = $(tag).width() / 1.65;//580;
    var color = d3.scaleOrdinal()
.range(['#E1004C', '#00A47F', '#FF6200', '#70E500', '#00644E']);
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
            .attr("xlink:href", "../../images/locater.svg")//'https://cdn3.iconfinder.com/data/icons/softwaredemo/PNG/24x24/DrawingPin1_Blue.png')
            .attr("transform", function (d) {
                return "translate(" + projection([d.long, d.lat]) + ")";
            });
    });
    d3.select(self.frameElement).style("height", height + "px");


}