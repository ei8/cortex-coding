using ei8.Cortex.Coding.Mirrors;
using neurUL.Common.Test;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ei8.Cortex.Coding.Test.Mirrors.MirrorImageSeriesFixture.Given
{
    public class Constants
    {
        public const string Url0 = "http://neurul.net/url0";
        public const string MirrorUrl0 = "http://neurul.net/mirrorUrl0";
        public const string Url1 = "http://neurul.net/url1";
        public const string MirrorUrl1 = "http://neurul.net/mirrorUrl1";
        public const string Url2 = "http://neurul.net/url2";
        public const string MirrorUrl2 = "http://neurul.net/mirrorUrl2";
        public const string Url3 = "http://neurul.net/url3";
        public const string MirrorUrl3 = "http://neurul.net/mirrorUrl3";

        public static readonly IEnumerable<MockMirrorImage> MirrorImages1Count = new MockMirrorImage[] {
            new MockMirrorImage()
            {
                Id = Guid.NewGuid(),
                Url = Given.Constants.Url1,
                Mirror = new MirrorInfo(Given.Constants.MirrorUrl1)
            }
        };

        public static readonly IEnumerable<MockMirrorImage> MirrorImages2Count = new MockMirrorImage[] {
            new MockMirrorImage() {
                Id = Guid.NewGuid(),
                Url = MirrorImageSeriesFixture.Given.Constants.Url1,
                Mirror = new MirrorInfo(MirrorImageSeriesFixture.Given.Constants.Url2)
            },
            new MockMirrorImage() {
                Id = Guid.NewGuid(),
                Url = MirrorImageSeriesFixture.Given.Constants.Url2,
                Mirror = new MirrorInfo(MirrorImageSeriesFixture.Given.Constants.MirrorUrl2)
            }
        }; 

        public static readonly IEnumerable<MockMirrorImage> MirrorImages3Count = new MockMirrorImage[] {
            new MockMirrorImage() {
                Id = Guid.NewGuid(),
                Url = Given.Constants.Url1,
                Mirror = new MirrorInfo(Given.Constants.Url2)
            },
            new MockMirrorImage() {
                Id = Guid.NewGuid(),
                Url = Given.Constants.Url2,
                Mirror = new MirrorInfo(Given.Constants.Url3)
            },
            new MockMirrorImage() {
                Id = Guid.NewGuid(),
                Url = Given.Constants.Url3,
                Mirror = new MirrorInfo(Given.Constants.MirrorUrl3)
            }
        };
    }

    namespace Constructing
    {
        public abstract class ConstructingContext : TestContext<MirrorImageSeries<MockMirrorImage>>
        {
            protected override bool InvokeWhenOnConstruct => false;

            protected override void When() => this.sut = new MirrorImageSeries<MockMirrorImage>(this.SeriesConstructorParameter);

            protected abstract IEnumerable<MockMirrorImage> SeriesConstructorParameter { get; }
        }

        public abstract class InvokeWhenConstructingContext : ConstructingContext
        {
            protected override bool InvokeWhenOnConstruct => true;
        }

        public class When_null_series : ConstructingContext
        {
            protected override IEnumerable<MockMirrorImage> SeriesConstructorParameter => null;

            [Fact]
            public void Then_should_throw_argument_null_exception()
            {
                Assert.Throws<ArgumentNullException>(this.When);
            }
        }

        public class When_empty_series : InvokeWhenConstructingContext
        {
            protected override IEnumerable<MockMirrorImage> SeriesConstructorParameter => Enumerable.Empty<MockMirrorImage>();

            [Fact]
            public void Then_should_contain_zero()
            {
                Assert.Equal(0, this.sut.Count);
            }
        }

        public class When_series_contains_one : InvokeWhenConstructingContext
        {
            protected override IEnumerable<MockMirrorImage> SeriesConstructorParameter => Constants.MirrorImages1Count;

            [Fact]
            public void Then_should_contain_one()
            {
                Assert.Single(this.sut);
            }

            [Fact]
            public void Then_should_contain_correct_urls()
            {
                Assert.Equal(MirrorImageSeriesFixture.Given.Constants.Url1, this.sut[0].Url);
                Assert.Equal(MirrorImageSeriesFixture.Given.Constants.MirrorUrl1, this.sut[0].Mirror.Url);
            }
        }

        public class When_series_contains_two_unrelated : ConstructingContext
        {
            protected override IEnumerable<MockMirrorImage> SeriesConstructorParameter => new MockMirrorImage[] {
                new MockMirrorImage() {
                    Id = Guid.NewGuid(),
                    Url = MirrorImageSeriesFixture.Given.Constants.Url1,
                    Mirror = new MirrorInfo(MirrorImageSeriesFixture.Given.Constants.MirrorUrl1)
                },
                new MockMirrorImage() {
                    Id = Guid.NewGuid(),
                    Url = MirrorImageSeriesFixture.Given.Constants.Url2,
                    Mirror = new MirrorInfo(MirrorImageSeriesFixture.Given.Constants.MirrorUrl2)
                }
            };

            [Fact]
            public void Then_should_throw_argument_exception()
            {
                Assert.Throws<ArgumentException>(this.When);
            }
        }

        public class When_series_contains_two_related : InvokeWhenConstructingContext
        {
            protected override IEnumerable<MockMirrorImage> SeriesConstructorParameter => Constants.MirrorImages2Count;

            [Fact]
            public void Then_should_contain_two()
            {
                Assert.Equal(2, this.sut.Count);
            }

            [Fact]
            public void Then_should_contain_correct_urls()
            {
                Assert.Equal(MirrorImageSeriesFixture.Given.Constants.Url1, this.sut[0].Url);
                Assert.Equal(MirrorImageSeriesFixture.Given.Constants.Url2, this.sut[0].Mirror.Url);
                Assert.Equal(MirrorImageSeriesFixture.Given.Constants.Url2, this.sut[1].Url);
                Assert.Equal(MirrorImageSeriesFixture.Given.Constants.MirrorUrl2, this.sut[1].Mirror.Url);
            }
        }

        public class When_series_contains_three_unrelated : ConstructingContext
        {
            protected override IEnumerable<MockMirrorImage> SeriesConstructorParameter =>
                new MockMirrorImage[] {
                    new MockMirrorImage() {
                        Id = Guid.NewGuid(),
                        Url = MirrorImageSeriesFixture.Given.Constants.Url1,
                        Mirror = new MirrorInfo(MirrorImageSeriesFixture.Given.Constants.Url2)
                    },
                    new MockMirrorImage() {
                        Id = Guid.NewGuid(),
                        Url = MirrorImageSeriesFixture.Given.Constants.Url2,
                        Mirror = new MirrorInfo(MirrorImageSeriesFixture.Given.Constants.MirrorUrl2)
                    },
                    new MockMirrorImage() {
                        Id = Guid.NewGuid(),
                        Url = MirrorImageSeriesFixture.Given.Constants.Url3,
                        Mirror = new MirrorInfo(MirrorImageSeriesFixture.Given.Constants.MirrorUrl3)
                    }
                };

            [Fact]
            public void Then_should_throw_argument_exception()
            {
                Assert.Throws<ArgumentException>(this.When);
            }
        }

        public class When_series_contains_three_related : InvokeWhenConstructingContext
        {
            protected override IEnumerable<MockMirrorImage> SeriesConstructorParameter => Constants.MirrorImages3Count;
            [Fact]
            public void Then_should_contain_three()
            {
                Assert.Equal(3, this.sut.Count);
            }

            [Fact]
            public void Then_should_contain_correct_urls()
            {
                Assert.Equal(MirrorImageSeriesFixture.Given.Constants.Url1, this.sut[0].Url);
                Assert.Equal(MirrorImageSeriesFixture.Given.Constants.Url2, this.sut[0].Mirror.Url);
                Assert.Equal(MirrorImageSeriesFixture.Given.Constants.Url2, this.sut[1].Url);
                Assert.Equal(MirrorImageSeriesFixture.Given.Constants.Url3, this.sut[1].Mirror.Url);
                Assert.Equal(MirrorImageSeriesFixture.Given.Constants.Url3, this.sut[2].Url);
                Assert.Equal(MirrorImageSeriesFixture.Given.Constants.MirrorUrl3, this.sut[2].Mirror.Url);
            }
        }
    }

    namespace Constructed
    {
        public abstract class ConstructedContext : TestContext<MirrorImageSeries<MockMirrorImage>>
        {
            protected override void Given() => this.sut = new MirrorImageSeries<MockMirrorImage>(this.SeriesConstructorParameter);

            protected abstract IEnumerable<MockMirrorImage> SeriesConstructorParameter { get; }
        }

        namespace With_series_containing_0_image
        {
            public abstract class WithSeriesContaining0ImageContext : ConstructedContext
            {
                protected override IEnumerable<MockMirrorImage> SeriesConstructorParameter => Enumerable.Empty<MockMirrorImage>();
            }

            public class When_adding
            {
                public abstract class WhenAddingContext : WithSeriesContaining0ImageContext
                {
                    protected override void When() => this.sut.Add(this.ItemMethodParameter);

                    protected abstract MockMirrorImage ItemMethodParameter { get; }
                }

                public class With_image : WhenAddingContext
                {
                    protected override MockMirrorImage ItemMethodParameter => new MockMirrorImage()
                    {
                        Id = Guid.NewGuid(),
                        Url = MirrorImageSeriesFixture.Given.Constants.Url1
                    };

                    [Fact]
                    public void Then_should_contain_1_image()
                    {
                        Assert.Equal(1, this.sut.Count);
                    }
                }
            }

            public class When_inserting
            {
                public abstract class WhenInsertingContext : WithSeriesContaining0ImageContext
                {
                    protected override void When() => this.sut.Insert(0, this.ItemMethodParameter);

                    protected abstract MockMirrorImage ItemMethodParameter { get; }
                }

                public class With_image : WhenInsertingContext
                {
                    protected override MockMirrorImage ItemMethodParameter => new MockMirrorImage()
                    {
                        Id = Guid.NewGuid(),
                        Url = MirrorImageSeriesFixture.Given.Constants.Url0,
                        Mirror = new MirrorInfo(Constants.Url1)
                    };

                    [Fact]
                    public void Then_should_contain_1_image()
                    {
                        Assert.Equal(1, this.sut.Count);
                    }
                }
            }

            public class When_removing
            {
                public abstract class WhenRemovingContext : WithSeriesContaining0ImageContext
                {
                    protected bool? methodResult = null;

                    protected override void When() => this.methodResult = this.sut.Remove(this.ItemMethodParameter);

                    protected abstract MockMirrorImage ItemMethodParameter { get; }
                }

                public class With_non_contained_image : WhenRemovingContext
                {
                    protected override MockMirrorImage ItemMethodParameter => new MockMirrorImage()
                    {
                        Id = Guid.NewGuid(),
                        Url = MirrorImageSeriesFixture.Given.Constants.Url0,
                        Mirror = new MirrorInfo(Constants.MirrorUrl0)
                    };

                    [Fact]
                    public void Then_should_return_false()
                    {
                        Assert.False(this.methodResult);
                    }
                }
            }
        }

        namespace With_series_containing_1_image
        {
            public abstract class WithSeriesContaining1ImageContext : ConstructedContext
            {
                protected override IEnumerable<MockMirrorImage> SeriesConstructorParameter => Constants.MirrorImages1Count;
            }

            public class When_adding
            {
                public abstract class WhenAddingContext : WithSeriesContaining1ImageContext
                {
                    protected override void When() => this.sut.Add(this.ItemMethodParameter);

                    protected abstract MockMirrorImage ItemMethodParameter { get; }
                }

                public class With_related_image : WhenAddingContext
                {
                    protected override MockMirrorImage ItemMethodParameter => new MockMirrorImage()
                    {
                        Id = Guid.NewGuid(),
                        Url = MirrorImageSeriesFixture.Given.Constants.MirrorUrl1
                    };

                    [Fact]
                    public void Then_should_contain_added_image()
                    {
                        Assert.Equal(2, this.sut.Count);
                    }
                }

                public class With_unrelated_image : WhenAddingContext
                {
                    protected override bool InvokeWhenOnConstruct => false;

                    protected override MockMirrorImage ItemMethodParameter => new MockMirrorImage()
                    {
                        Id = Guid.NewGuid(),
                        Url = MirrorImageSeriesFixture.Given.Constants.Url2
                    };

                    [Fact]
                    public void Then_should_throw_argument_exception() => Assert.Throws<ArgumentException>(this.When);
                }
            }

            public class When_inserting
            {
                public abstract class WhenInsertingContext : WithSeriesContaining1ImageContext
                {
                    protected override void When() => this.sut.Insert(0, this.ItemMethodParameter);

                    protected abstract MockMirrorImage ItemMethodParameter { get; }
                }

                public class With_related_image : WhenInsertingContext
                {
                    protected override MockMirrorImage ItemMethodParameter => new MockMirrorImage()
                    {
                        Id = Guid.NewGuid(),
                        Url = MirrorImageSeriesFixture.Given.Constants.Url0,
                        Mirror = new MirrorInfo(Constants.Url1)
                    };

                    [Fact]
                    public void Then_should_contain_added_image()
                    {
                        Assert.Equal(2, this.sut.Count);
                    }
                }

                public class With_unrelated_image : WhenInsertingContext
                {
                    protected override bool InvokeWhenOnConstruct => false;

                    protected override MockMirrorImage ItemMethodParameter => new MockMirrorImage()
                    {
                        Id = Guid.NewGuid(),
                        Url = MirrorImageSeriesFixture.Given.Constants.Url0,
                        Mirror = new MirrorInfo(Constants.MirrorUrl0)
                    };

                    [Fact]
                    public void Then_should_throw_argument_exception() => Assert.Throws<ArgumentException>(this.When);
                }
            }

            public class When_removing
            {
                public abstract class WhenRemovingContext : WithSeriesContaining1ImageContext
                {
                    protected bool? methodResult = null;

                    protected override void When() => this.methodResult = this.sut.Remove(this.ItemMethodParameter);

                    protected abstract MockMirrorImage ItemMethodParameter { get; }
                }

                public class With_contained_image : WhenRemovingContext
                {
                    protected override MockMirrorImage ItemMethodParameter => this.sut[0];

                    [Fact]
                    public void Then_should_contain_0()
                    {
                        Assert.Equal(0, this.sut.Count);
                    }

                    [Fact]
                    public void Then_should_return_true()
                    {
                        Assert.True(this.methodResult);
                    }
                }

                public class With_non_contained_image : WhenRemovingContext
                {
                    protected override MockMirrorImage ItemMethodParameter => new MockMirrorImage()
                    {
                        Id = Guid.NewGuid(),
                        Url = MirrorImageSeriesFixture.Given.Constants.Url0,
                        Mirror = new MirrorInfo(Constants.MirrorUrl0)
                    };

                    [Fact]
                    public void Then_should_return_false()
                    {
                        Assert.False(this.methodResult);
                    }
                }
            }
        }

        namespace With_series_containing_2_images
        {
            public abstract class WithSeriesContaining2ImagesContext : ConstructedContext
            {
                protected override IEnumerable<MockMirrorImage> SeriesConstructorParameter => Constants.MirrorImages2Count;
            }

            public class When_adding
            {
                public abstract class WhenAddingContext : WithSeriesContaining2ImagesContext
                {
                    protected override void When() => this.sut.Add(this.ItemMethodParameter);

                    protected abstract MockMirrorImage ItemMethodParameter { get; }
                }

                public class With_related_image : WhenAddingContext
                {
                    protected override MockMirrorImage ItemMethodParameter => new MockMirrorImage()
                    {
                        Id = Guid.NewGuid(),
                        Url = MirrorImageSeriesFixture.Given.Constants.MirrorUrl2
                    };

                    [Fact]
                    public void Then_should_contain_added_image()
                    {
                        Assert.Equal(3, this.sut.Count);
                    }
                }

                public class With_unrelated_image : WhenAddingContext
                {
                    protected override bool InvokeWhenOnConstruct => false;

                    protected override MockMirrorImage ItemMethodParameter => new MockMirrorImage()
                    {
                        Id = Guid.NewGuid(),
                        Url = MirrorImageSeriesFixture.Given.Constants.Url3
                    };

                    [Fact]
                    public void Then_should_throw_argument_exception() => Assert.Throws<ArgumentException>(this.When);
                }
            }

            public class When_inserting
            {
                public abstract class WhenInsertingContext : WithSeriesContaining2ImagesContext
                {
                    protected override bool InvokeWhenOnConstruct => false;

                    protected override void When() => this.sut.Insert(1, this.ItemMethodParameter);

                    protected abstract MockMirrorImage ItemMethodParameter { get; }
                }

                public class With_related_image : WhenInsertingContext
                {
                    protected override MockMirrorImage ItemMethodParameter => new MockMirrorImage()
                    {
                        Id = Guid.NewGuid(),
                        Url = MirrorImageSeriesFixture.Given.Constants.Url0,
                        Mirror = new MirrorInfo(Constants.Url1)
                    };

                    [Fact]
                    public void Then_should_throw_argument_exception() => Assert.Throws<ArgumentException>(this.When);
                }

                public class With_unrelated_image : WhenInsertingContext
                {
                    protected override MockMirrorImage ItemMethodParameter => new MockMirrorImage()
                    {
                        Id = Guid.NewGuid(),
                        Url = MirrorImageSeriesFixture.Given.Constants.Url0,
                        Mirror = new MirrorInfo(Constants.MirrorUrl0)
                    };

                    [Fact]
                    public void Then_should_throw_argument_exception() => Assert.Throws<ArgumentException>(this.When);
                }
            }

            public class When_removing
            {
                public abstract class WhenRemovingContext : WithSeriesContaining2ImagesContext
                {
                    protected bool? methodResult = null;

                    protected override void When() => this.methodResult = this.sut.Remove(this.ItemMethodParameter);

                    protected abstract MockMirrorImage ItemMethodParameter { get; }
                }

                public class With_contained_image : WhenRemovingContext
                {
                    protected override MockMirrorImage ItemMethodParameter => this.sut[1];

                    [Fact]
                    public void Then_should_reduce_count_by_1()
                    {
                        Assert.Equal(1, this.sut.Count);
                    }

                    [Fact]
                    public void Then_should_return_true()
                    {
                        Assert.True(this.methodResult);
                    }
                }

                public class With_non_contained_image : WhenRemovingContext
                {
                    protected override MockMirrorImage ItemMethodParameter => new MockMirrorImage()
                    {
                        Id = Guid.NewGuid(),
                        Url = MirrorImageSeriesFixture.Given.Constants.Url0,
                        Mirror = new MirrorInfo(Constants.MirrorUrl0)
                    };

                    [Fact]
                    public void Then_should_return_false()
                    {
                        Assert.False(this.methodResult);
                    }
                }
            }
        }
    }
}