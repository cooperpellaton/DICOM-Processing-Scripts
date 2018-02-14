% J. Tremel (tremeljosh@gmail.com), 2009, University of Pittsburgh

function logisticFit(a,static,shuffle)
%TEST    Create plot of datasets and fits
%   TEST(A,STATIC,SHUFFLE)
%   Creates a plot, similar to the plot in the main curve fitting
%   window, using the data that you provide as input.  You can
%   apply this function to the same data you used with cftool
%   or with different data.  You may want to edit the function to
%   customize the code and this help message.
%
%   Number of datasets:  2
%   Number of fits:  2

 
% Data from dataset "static vs. a":
%    X = a:
%    Y = static:
%    Unweighted
 
% Data from dataset "shuffle vs. a":
%    X = a:
%    Y = shuffle:
%    Unweighted
%
% This function was automatically generated on 17-Nov-2009 13:23:31

% Set up figure to receive datasets and fits
f_ = clf;
figure(f_);
set(f_,'Units','Pixels','Position',[906 352 672 481]);
xlim_ = [Inf -Inf];       % limits of x axis
ax_ = axes;
set(ax_,'Units','normalized','OuterPosition',[0 0 1 1]);
set(ax_,'Box','on');
axes(ax_); hold on;

 
% --- Plot data originally in dataset "static vs. a"
a = [1; 2; 3; 4; 5; 6; 7];
static = [0.03811; 0.20145; 0.38113; 0.58076; 0.74047; 0.89292; 1];
h_ = line(a,static,'Parent',ax_,'Color',[0.333333 0 0.666667],...
     'LineStyle','none', 'LineWidth',1,...
     'Marker','.', 'MarkerSize',12);
xlim_(1) = min(xlim_(1),min(a));
xlim_(2) = max(xlim_(2),max(a));
 
% --- Plot data originally in dataset "shuffle vs. a"
shuffle = [0.01402; 0.07944; 0.1729; 0.36449; 0.57944; 0.81308; 1];
h_ = line(a,shuffle,'Parent',ax_,'Color',[0.333333 0.666667 0],...
     'LineStyle','none', 'LineWidth',1,...
     'Marker','.', 'MarkerSize',12);
xlim_(1) = min(xlim_(1),min(a));
xlim_(2) = max(xlim_(2),max(a));

% Nudge axis limits beyond data limits
if all(isfinite(xlim_))
   xlim_ = xlim_ + [-1 1] * 0.01 * diff(xlim_);
   set(ax_,'XLim',xlim_)
end


% --- Create fit "logistic"
ok_ = isfinite(a) & isfinite(static);
st_ = [0.8557282171647 0.16459467626 0.07934804325228 0.6282581102934 0.2866118507107 ];
ft_ = fittype('e/(a*exp(-b*x)+c)+d',...
     'dependent',{'y'},'independent',{'x'},...
     'coefficients',{'a', 'b', 'c', 'd', 'e'});

% Fit this model using new data
cf_ = fit(a(ok_),static(ok_),ft_,'Startpoint',st_);

% Or use coefficients from the original fit:
if 0
   cv_ = { -8.266906507707, 0.4905564843476, -1.67349945329, -0.3486286591014, -2.615152535018};
   cf_ = cfit(ft_,cv_{:});
end

% Plot this fit
h_ = plot(cf_,'fit',0.95);
legend off;  % turn off legend from plot method call
set(h_(1),'Color',[1 0 0],...
     'LineStyle','-', 'LineWidth',2,...
     'Marker','none', 'MarkerSize',6);

% --- Create fit "logistic copy 1"
fo_ = fitoptions('method','NonlinearLeastSquares','Normalize','on');
ok_ = isfinite(a) & isfinite(shuffle);
st_ = [0.8557282171647 0.16459467626 0.07934804325228 0.6282581102934 0.2866118507107 ];
set(fo_,'Startpoint',st_);
ft_ = fittype('e/(a*exp(-b*x)+c)+d',...
     'dependent',{'y'},'independent',{'x'},...
     'coefficients',{'a', 'b', 'c', 'd', 'e'});

% Fit this model using new data
cf_ = fit(a(ok_),shuffle(ok_),ft_,fo_);

% Or use coefficients from the original fit:
if 0
   cv_ = { 8.870184164052, 1.590478489422, 3.951698018529, -0.04580698603916, 5.14591854831};
   cf_ = cfit(ft_,cv_{:});
end

% Plot this fit
h_ = plot(cf_,'fit',0.95);
legend off;  % turn off legend from plot method call
set(h_(1),'Color',[0 0 1],...
     'LineStyle','-', 'LineWidth',2,...
     'Marker','none', 'MarkerSize',6);

% Done plotting data and fits.  Now finish up loose ends.
hold off;
