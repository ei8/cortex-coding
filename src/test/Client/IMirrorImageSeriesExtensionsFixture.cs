using ei8.Cortex.Coding.Mirrors;
using ei8.Cortex.Coding.Test.Mirrors;
using neurUL.Common.Test;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ei8.Cortex.Coding.Client.Test.Mirrors.IMirrorImageSeriesExtensionsFixture.Given
{
    namespace Constructed
    {
        public abstract class ConstructedContext : TestContext<IMirrorImageSeries<MockMirrorImage>>
        {
            protected override void Given() => this.sut = new MirrorImageSeries<MockMirrorImage>(this.SeriesConstructorParameter);

            protected abstract IEnumerable<MockMirrorImage> SeriesConstructorParameter { get; }
        }

        namespace With_series_containing_2_images
        {
            public abstract class WithSeriesContaining2ImagesContext : ConstructedContext
            {
                protected override IEnumerable<MockMirrorImage> SeriesConstructorParameter => Coding.Test.Mirrors.MirrorImageSeriesFixture.Given.Constants.MirrorImages2Count;
            }

            public class When_deserializing
            {
                public abstract class WhenDeserializingContext : WithSeriesContaining2ImagesContext
                {
                    protected override void When()
                    {
                        var value = new IMirrorImageSeries<MockMirrorImage>[] { this.sut };

                        var so = value.ToJson();
                        var @do = IMirrorImageSeriesExtensions.CreateList<MockMirrorImage>();
                        @do.ReadJson(so);
                        this.sut = @do.First();
                    }
                }

                public class With_valid_data : WhenDeserializingContext
                {
                    [Fact]
                    public void Then_should_contain_same_data()
                    {
                        Assert.Equal(2, this.sut.Count);
                        Assert.IsType<MirrorImageSeries<MockMirrorImage>>(this.sut);
                        var item = ((MirrorImageSeries<MockMirrorImage>)this.sut)[0];
                        Assert.Equal(Coding.Test.Mirrors.MirrorImageSeriesFixture.Given.Constants.Url1, item.Url);
                        Assert.Equal(Coding.Test.Mirrors.MirrorImageSeriesFixture.Given.Constants.Url2, item.Mirror.Url);

                        item = ((MirrorImageSeries<MockMirrorImage>)this.sut)[1];
                        Assert.Equal(Coding.Test.Mirrors.MirrorImageSeriesFixture.Given.Constants.Url2, item.Url);
                        Assert.Equal(Coding.Test.Mirrors.MirrorImageSeriesFixture.Given.Constants.MirrorUrl2, item.Mirror.Url);
                    }
                }
            }
        }
    }
}